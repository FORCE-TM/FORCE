using FORCE.Core.Plugins.Commands;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

public abstract class ForcePlugin
{
    public virtual Task OnPluginLoadAsync(bool reload) => Task.CompletedTask;
    public virtual Task OnPluginUnloadAsync(bool reload) => Task.CompletedTask;

    public ForceController Force { get; private set; }
    public TmServer Server => Force.Server;

    protected PluginInfo Plugin { get; private set; }
    protected CommandContext Command { get; private set; }

    internal void UseTheForce(ForceController force)
    {
        Force = force;
    }

    internal void SetPlugin(PluginInfo plugin)
    {
        Plugin = plugin;
    }

    internal void SetCommandContext(CommandContext commandContext)
    {
        if (Force == null)
            throw new($"{nameof(UseTheForce)} must be called first.");

        Command = commandContext;
    }
}
