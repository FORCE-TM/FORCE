namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates the <see cref="Name"/>, <see cref="Version"/> and <see cref="Author"/> of a FORCE plugin.<br/>
/// This attribute is required in order for a plugin to be discoverable.<br/>
/// This same class must also inherit from <see cref="PluginBase"/>.<br/>
/// There can only be one plugin class per assembly, or at least per assembly module.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PluginAttribute : Attribute, IPluginAttribute
{
    /// <inheritdoc cref="PluginAttribute"/>
    /// <param name="name">Display name of the plugin.</param>
    /// <param name="version">Version of the plugin. This must be a valid <see cref="System.Version"/> string.</param>
    /// <param name="author">Display name of the plugin author. Optional.</param>
    public PluginAttribute(string name, string version, string? author = null)
    {
        Name = name;
        Version = new Version(version);
        Author = author;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public Version Version { get; }

    /// <inheritdoc/>
    public string? Author { get; }
}

public interface IPluginAttribute
{
    /// <summary>
    /// The plugin display name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The plugin version.
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// The plugin author. Nullable.
    /// </summary>
    public string? Author { get; }
}
