namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that a class is a FORCE plugin.<br/>
/// This must be used for a plugin to be discoverable.<br/>
/// This same class must also inherit from <see cref="PluginBase"/>.<br/>
/// There can only be one per assembly, or at least per assembly module.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PluginAttribute : Attribute, IPluginAttribute
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

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Version Version { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? Author { get; set; }
}

public interface IPluginAttribute
{
    /// <summary>
    /// The plugin display name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The plugin version.
    /// </summary>
    public Version Version { get; set; }

    /// <summary>
    /// The plugin author. Nullable.
    /// </summary>
    public string? Author { get; set; }
}
