namespace FORCE.Core.Plugin;

/// <summary>
/// Adds a summary to your plugin, command group, command or command parameter. This is optional.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class SummaryAttribute : Attribute, ISummaryAttribute
{
    /// <inheritdoc cref="SummaryAttribute"/>
    public SummaryAttribute(string text)
    {
        Summary = text;
    }

    /// <inheritdoc/>
    public string? Summary { get; }
}

public interface ISummaryAttribute
{
    public string? Summary { get; }
}
