namespace FORCE.Core.Plugin;

/// <summary>
/// Adds a summary to your plugin, command group, command or command parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class SummaryAttribute : Attribute, ISummaryAttribute
{
    /// <summary>
    /// <inheritdoc cref="SummaryAttribute"/>
    /// </summary>
    public SummaryAttribute(string text)
    {
        Summary = text;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? Summary { get; set; }
}

public interface ISummaryAttribute
{
    public string? Summary { get; set; }
}
