using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

public abstract class PluginContext
{
    public ForceController Force { get; internal set; }
    public TmServer Server => Force.Server;

    public PluginInfo Plugin { get; internal set; }
}
