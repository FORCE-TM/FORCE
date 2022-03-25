using FORCE.Core.Plugins.Commands;

namespace FORCE.Core.Plugins;

public abstract class ForcePlugin : PluginContext
{
    public CommandContext Command { get; internal set; }

    public virtual Task OnPluginLoadAsync(bool reload) => Task.CompletedTask;
    public virtual Task OnPluginUnloadAsync(bool reload) => Task.CompletedTask;
}
