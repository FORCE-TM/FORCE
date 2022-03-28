using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

public class ContextBase
{
    /// <inheritdoc cref="PluginDisplayModel"/>
    public PluginDisplayModel Plugin { get; set; }

    /// <inheritdoc cref="TmServer"/>
    public TmServer Server { get; set; }
}
