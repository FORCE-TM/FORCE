namespace FORCE.Core.Plugin;

public abstract class PluginBase : ContextBase
{
    /// <summary>
    /// Method invoked when the plugin was loaded, and is ready to process commands.<br/>
    /// This method can be used to subscribe to some <see cref="ContextBase.Server"/> events.
    /// </summary>
    /// <param name="reload">Whether the plugin was just reloaded or not.</param>
    public virtual Task OnPluginLoadAsync(bool reload)
        => Task.CompletedTask;

    /// <summary>
    /// Method invoked when the plugin is being unloaded.<br/>
    /// Do not worry about unsubscribing from events, this is already being done behind the scenes.
    /// </summary>
    /// <param name="reload">Whether the plugin is being reloaded or not.</param>
    public virtual Task OnPluginUnloadAsync(bool reload)
        => Task.CompletedTask;
}
