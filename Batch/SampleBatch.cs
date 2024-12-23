using Batch.Attributes;

namespace Batch;

[RegisterCommands(nameof(SampleBatch))]
public class SampleBatch {
    /// <summary>
    /// 19時に実行
    /// </summary>
    [Batch("0 10 * * *")]
    public void Run() {
        Console.WriteLine("Hello World!");
    }
}
