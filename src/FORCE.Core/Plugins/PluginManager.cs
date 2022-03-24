using System.Runtime.Loader;
using FORCE.Core.Plugins.Commands;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

internal class PluginManager
{
    public event Action<PluginInfo> OnPluginLoaded;
    public event Action<PluginInfo> OnPluginUnloaded;

    public ForceController Force { get; }
    public List<PluginAssembly> PluginAssemblies { get; }
    public CommandHandler CommandHandler { get; }

    public PluginManager(ForceController force)
    {
        Force = force;
        PluginAssemblies = new();
        CommandHandler = new CommandHandler(this);
        CommandHandler.StartListening();
    }

    public bool LoadPluginAssembly(PluginAssembly pluginAssembly)
    {
        if (pluginAssembly.Plugins.Count == 0)
            return false;

        PluginAssemblies.Add(pluginAssembly);

        foreach (var plugin in pluginAssembly.Plugins)
        {
            plugin.MainInstance.OnPluginLoadAsync();
            OnPluginLoaded?.Invoke(plugin);
        }

        return true;
    }

    public bool UnloadPluginAssembly(PluginAssembly pluginAssembly, bool reload = false)
    {
        bool removed = PluginAssemblies.Remove(pluginAssembly);

        if (removed)
        {
            foreach (var plugin in pluginAssembly.Plugins)
            {
                pluginAssembly.AssemblyLoadContext.Unload();

                if (!reload)
                {
                    try
                    {
                        plugin.MainInstance.OnPluginUnloadAsync();
                    }
                    catch (NotImplementedException)
                    {
                        // ignored
                    }

                    OnPluginUnloaded?.Invoke(plugin);
                }

                // Only after OnPluginUnloadAsync, to make sure the plugin can not register any new event
                pluginAssembly.RemoveAllEventHandlers(Force.Server);
            }
        }

        return removed;
    }

    public bool ReloadPluginAssembly(PluginAssembly pluginAssembly)
    {
        bool unloaded = UnloadPluginAssembly(pluginAssembly, true);

        if (unloaded)
        {
            pluginAssembly = GetPluginAssemblyFromPath(pluginAssembly.Assembly.Location);

            LoadPluginAssembly(pluginAssembly);
        }

        return unloaded;
    }

    public static string[] GetPluginFilesFromDirectory(DirectoryInfo directory, SearchOption searchOption = SearchOption.AllDirectories)
    {
        return Directory.GetFiles(directory.FullName, "*.dll", searchOption);
    }

    public PluginAssembly GetPluginAssemblyFromPath(string assemblyPath)
    {
        var assemblyLoadContext = new AssemblyLoadContext(null, isCollectible: true);
        var assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

        return new PluginAssembly(this, assemblyLoadContext, assembly);
    }
}
