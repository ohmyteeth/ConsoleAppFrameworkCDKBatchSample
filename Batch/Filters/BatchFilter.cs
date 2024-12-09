using Microsoft.Extensions.Logging;

namespace Batch.Filters;

/// <summary>
/// ログを出力するフィルタ
/// </summary>
public class BatchFilter : ConsoleAppFilter {
    public override async ValueTask Invoke(ConsoleAppContext context, Func<ConsoleAppContext, ValueTask> next) {
        var batchName = context.MethodInfo.DeclaringType!.Name + context.MethodInfo.Name;
        context.Logger.LogInformation("{name} Batch started. {time}", batchName, context.Timestamp.ToLocalTime());
        try {
            await next(context);
            context.Logger.LogInformation("{name} Batch completed. {time} Elapsed: {elapsed}", batchName, DateTimeOffset.Now, DateTimeOffset.UtcNow - context.Timestamp);
        } catch (Exception e) {
            context.Logger.LogError(e, "{name} Batch failed. {time} Elapsed: {elapsed}", batchName, DateTimeOffset.Now, DateTimeOffset.UtcNow - context.Timestamp);
            throw;
        }
    }
}