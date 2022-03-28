namespace FORCE.Core.Plugin;

/// <summary>
/// Adds a summary to your plugin, command group, command or command parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
public class SummaryAttribute : Attribute, ISummaryAttribute
{
    /// <summary>
    /// <inheritdoc cref="SummaryAttribute"/>
    /// </summary>
    public SummaryAttribute(string text)
    {
        Text = text;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string Text { get; set; }
}

public interface ISummaryAttribute
{
    public string Text { get; set; }
}
