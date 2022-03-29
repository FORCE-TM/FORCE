using FORCE.Core.Plugin;

namespace FORCE.Core.Models;

internal class CommandParameterInfo : ISummaryAttribute, IRemainderAttribute
{
    public Type Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public object? DefaultValue { get; set; }
    public bool HasDefaultValue { get; set; }
    public string? Summary { get; set; }
    public bool IsRemainder { get; set; }
}
