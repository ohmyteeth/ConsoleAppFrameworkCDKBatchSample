using Batch.Attributes;

namespace Batch;

public class SampleBatch : ConsoleAppBase {
    /// <summary>
    /// 19時に実行
    /// </summary>
    [Batch("0 10 * * *")]
    public void Run() {
        Console.WriteLine("Hello World!");
    }
}
