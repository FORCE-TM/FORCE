namespace FORCE.Core.Plugin.Models;

internal class CommandParameterInfo : IUsageAttribute, ISummaryAttribute, IRemainderAttribute
{
    public Type Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public object? DefaultValue { get; set; }
    public bool HasDefaultValue { get; set; }
    public string? UsageName { get; set; }
    public string? Summary { get; set; }
    public bool IsRemainder { get; set; }
}
