using FORCE.Core.Plugins.Commands;

namespace FORCE.Core.Plugins;

public abstract class ForcePlugin : ForceCommand
{
    public virtual Task OnPluginLoadAsync() => Task.CompletedTask;
    public virtual Task OnPluginUnloadAsync() => Task.CompletedTask;

    protected ForceController Force { get; private set; }
    protected TmServer Server => Force.Server;

    internal void UseTheForce(ForceController force)
    {
        Force = force;
    }
}
