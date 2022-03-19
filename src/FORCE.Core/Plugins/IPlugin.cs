namespace FORCE.Core.Plugins;

public interface IPlugin
{
    public Task OnPluginLoadAsync(ForceController force);
    public Task OnPluginUnloadAsync(ForceController force);
}
