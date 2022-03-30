namespace FORCE.Core.Plugin;

/// <summary>
/// Overrides the usage name of a command parameter, which will be shown in a command usage help.<br/>
/// By default, the usage name of a parameter is its variable name.<br/>
/// Example of a useful case: <c>/myplugin status &lt;on|off&gt;</c>
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class UsageAttribute : Attribute, IUsageAttribute
{
    /// <inheritdoc cref="UsageAttribute"/>
    /// <param name="name">Custom usage name of a command parameter.</param>
    public UsageAttribute(string name)
    {
        UsageName = name;
    }

    /// <inheritdoc/>
    public string? UsageName { get; }
}

public interface IUsageAttribute
{
    /// <summary>
    /// The custom usage name of a command parameter.
    /// </summary>
    public string? UsageName { get; }
}
