using Amazon.CDK;
using Amazon.CDK.AWS.Ecr.Assets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;
using Constructs;
using Batch.Attributes;
using System.Reflection;

namespace Stack;

internal class BatchStack : Amazon.CDK.Stack {

    public static BatchStack Create(Construct scope, string stackName) {
        return new BatchStack(scope, stackName);
    }

    public BatchStack(Construct scope, string stackName) : base(scope, null, new StackProps {
        StackName = stackName,
    }) {
        _ = new LogGroup(this, "BatchLogGroup", new LogGroupProps {
            LogGroupName = StackName,
            Retention = RetentionDays.SIX_MONTHS,
        });

        var role = new Role(this, "BatchRole", new RoleProps {
            RoleName = $"TaskExecutionRole",
            AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
            InlinePolicies = new Dictionary<string, PolicyDocument>() {
                ["BatchExecutionPolicy"] = new PolicyDocument(new PolicyDocumentProps {
                    Statements = [
                        new PolicyStatement(new PolicyStatementProps {
                            Actions = [
                                "logs:CreateLogStream",
                                "logs:PutLogEvents",
                                "logs:DescribeLogStreams",
                                "logs:DescribeLogGroups",
                            ],
                            Resources = [
                                "*",
                            ],
                        }),
                    ],
                }),
            }
        });

        var dockerImage = new DockerImageAsset(this, "DockerImage", new DockerImageAssetProps {
            Directory = "../Batch",
        });

        var batches = typeof(BatchAttribute).Assembly.GetTypes()
            .Where(t => t.Namespace?.StartsWith("Batch") ?? false)
            .ToList();

        // Batch属性がついているメソッドをすべて取得してBatchFunctionを作成
        foreach (var batch in batches) {
            var methods = batch.GetMethods()
             .Where(m => m.GetCustomAttribute<BatchAttribute>() != null)
             .Select(method => {
                 var batchAttribute = method.GetCustomAttribute<BatchAttribute>()!;
                 return (method, batchAttribute);
             })
             .ToList();
            foreach (var method in methods) {
                _ = new BatchFunction(this, StackName, batch.Name, method.method.Name, method.batchAttribute.CronFormula, role, dockerImage.Repository, dockerImage.ImageTag);
            }
        }
    }
}