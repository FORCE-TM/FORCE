using System.Runtime.Loader;
using FORCE.Core.Plugins.Commands;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

internal class PluginManager
{
    public event Action<PluginInfo, bool> OnPluginLoaded;
    public event Action<PluginInfo, bool> OnPluginUnloaded;

    public ForceController Force { get; }
    public List<PluginAssembly> PluginAssemblies { get; }
    public CommandHandler CommandHandler { get; }

    public PluginManager(ForceController force)
    {
        Force = force;
        PluginAssemblies = new();
        CommandHandler = new CommandHandler(this);
    }

    public bool LoadPluginAssembly(PluginAssembly pluginAssembly, bool reload = false)
    {
        if (pluginAssembly.Plugins.Count == 0)
            return false;

        PluginAssemblies.Add(pluginAssembly);

        foreach (var plugin in pluginAssembly.Plugins)
        {
            plugin.MainInstance.OnPluginLoadAsync(reload);
            OnPluginLoaded?.Invoke(plugin, reload);
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

                try
                {
                    plugin.MainInstance.OnPluginUnloadAsync(reload);
                }
                catch (NotImplementedException)
                {
                    // ignored
                }

                OnPluginUnloaded?.Invoke(plugin, reload);

                // Only after OnPluginUnload, to make sure the plugin can not register any new event
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
