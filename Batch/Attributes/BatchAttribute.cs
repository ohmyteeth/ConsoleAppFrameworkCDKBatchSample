namespace Batch.Attributes;

/// <summary>
/// このメソッドがバッチであることを示す属性
/// </summary>
/// <param name="CronFormula">Cron式 UTC</param>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class BatchAttribute(string CronFormula) : Attribute {
    public string CronFormula { get; } = CronFormula;
}