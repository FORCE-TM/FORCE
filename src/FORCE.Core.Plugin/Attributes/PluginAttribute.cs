namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that a class is a FORCE plugin.<br/>
/// This must be used for a plugin to be discovered.<br/>
/// The same class must also implement <see cref="Plugin"/>.<br/>
/// There can only be one per assembly, or at least per "module".
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PluginAttribute : Attribute, IPluginAttribute
{
    /// <summary>
    /// <inheritdoc cref="PluginAttribute"/>
    /// </summary>
    /// <param name="name">Display name of the plugin.</param>
    /// <param name="version">Version of the plugin. This must be a valid <see cref="System.Version"/> string.</param>
    /// <param name="author">Display name of the plugin author. Optional.</param>
    public PluginAttribute(string name, string version, string? author = null)
    {
        Name = name;
        Version = new Version(version);
        Author = author;
    }

    public string Name { get; set; }
    public Version Version { get; set; }
    public string? Author { get; set; }
}

public interface IPluginAttribute
{
    public string Name { get; set; }
    public Version Version { get; set; }
    public string? Author { get; set; }
}
