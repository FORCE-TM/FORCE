using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

/// <summary>
/// Class containing some objects for plugins to interact with.<br/>
/// <b>⚠️ You are NOT supposed to inherit this! This is for internal purposes only.</b><br/>
/// Perhaps you may be looking for <see cref="PluginBase"/> or even <see cref="CommandContext"/> instead?
/// </summary>
public class ContextBase
{
    /// <inheritdoc cref="PluginDisplayInfo"/>
    public PluginDisplayInfo Plugin { get; set; }

    /// <inheritdoc cref="TmServer"/>
    public TmServer Server { get; set; }

    /// <inheritdoc cref="Shared.ColorScheme"/>
    public ColorScheme ColorScheme { get; set; }
}
