using System.Diagnostics.CodeAnalysis;
using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

public class ContextBase
{
    /// <inheritdoc cref="PluginDisplayModel"/>
    [MaybeNull]
    public PluginDisplayModel Plugin { get; set; }

    /// <inheritdoc cref="TmServer"/>
    [MaybeNull] 
    public TmServer Server { get; set; }
}
