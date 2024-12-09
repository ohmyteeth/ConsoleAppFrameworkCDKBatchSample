using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.Events;
using Stack.Extensions;

namespace Stack;

internal class BatchFunction {
    public BatchFunction(Construct scope, string stackName, string batchName, string functionName, string cron, IRole executionRole, IRepository ecr, string imageDigest) {

        var baseName = batchName + functionName;
        var function = new Function(scope, baseName + "Function", new FunctionProps {
            Architecture = Architecture.X86_64,
            Code = Code.FromEcrImage(ecr, new EcrImageCodeProps {
                Cmd = [batchName, functionName],
                TagOrDigest = imageDigest,
            }),
            FunctionName = (stackName + baseName).ToKebabCase(),
            Handler = Handler.FROM_IMAGE,
            Runtime = Runtime.FROM_IMAGE,
            Role = executionRole,
            MemorySize = 512,
            Timeout = Duration.Minutes(15),
            RetryAttempts = 0,
            LogGroup = null,
        });

        // EventBridgeのスケジュールルールを作成
        var rule = new Rule(scope, baseName + "Rule", new RuleProps {
            Enabled = true,
            Schedule = Schedule.Expression($"cron({cron})"),
            Targets = [
                new LambdaFunction(function)
            ]
        });

        // EventBridgeからLambdaを呼び出すための権限を追加
        function.AddPermission(baseName + "Permission", new Permission {
            Principal = new ServicePrincipal("events.amazonaws.com"),
            SourceArn = rule.RuleArn,
            Action = "lambda:InvokeFunction",
        });
    }
}
