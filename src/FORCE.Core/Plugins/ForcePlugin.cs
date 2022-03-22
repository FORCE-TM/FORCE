using FORCE.Core.Plugins.Commands;

namespace FORCE.Core.Plugins;

public abstract class ForcePlugin
{
    public virtual Task OnPluginLoadAsync() => Task.CompletedTask;
    public virtual Task OnPluginUnloadAsync() => Task.CompletedTask;

    public ForceController Force { get; private set; }
    public TmServer Server => Force.Server;

    protected CommandContext Command { get; private set; }

    internal void UseTheForce(ForceController force)
    {
        Force = force;
    }

    internal void SetCommandContext(CommandContext context)
    {
        if (Force == null)
            throw new Exception($"{nameof(UseTheForce)} must be called first.");

        Command = context;
        Command.SetTmServer(Force.Server);
    }
}
