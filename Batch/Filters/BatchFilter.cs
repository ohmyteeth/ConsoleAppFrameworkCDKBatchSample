using Microsoft.Extensions.Logging;

namespace Batch.Filters;

/// <summary>
/// ログを出力するフィルタ
/// </summary>
internal class BatchFilter(ConsoleAppFilter next, ILoggerFactory LoggerFactory) : ConsoleAppFilter(next) {

    public override async Task InvokeAsync(ConsoleAppContext context, CancellationToken cancellationToken) {

        var timestamp = DateTimeOffset.UtcNow;
        var batchName = $"{context.Arguments[0]} {context.Arguments[1]}";
        var Logger = LoggerFactory.CreateLogger(batchName);
        Logger.LogInformation("{name} Batch started. {time}", batchName, timestamp.ToLocalTime());
        try {
            await Next.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            Logger.LogInformation("{name} Batch completed. {time} Elapsed: {elapsed}", batchName, DateTimeOffset.Now, DateTimeOffset.UtcNow - timestamp);
        } catch (Exception e) {
            Logger.LogError(e, "{name} Batch failed. {time} Elapsed: {elapsed}", batchName, DateTimeOffset.Now, DateTimeOffset.UtcNow - timestamp);
            throw;
        }
    }
}