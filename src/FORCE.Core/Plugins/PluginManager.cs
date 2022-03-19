using System.Runtime.Loader;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

internal class PluginManager
{
    public List<PluginAssembly> PluginAssemblies { get; } = new();

    private readonly ForceController _force;

    public PluginManager(ForceController force)
    {
        _force = force;
    }

    public bool LoadPlugins(PluginAssembly pluginAssembly)
    {
        if (pluginAssembly.Plugins.Count == 0)
            return false;

        PluginAssemblies.Add(pluginAssembly);

        foreach (var plugin in pluginAssembly.Plugins)
        {
            plugin.Value.OnPluginLoadAsync(_force);
        }

        return true;
    }

    public bool UnloadPlugins(PluginAssembly pluginAssembly)
    {
        bool removed = PluginAssemblies.Remove(pluginAssembly);

        if (removed)
        {
            pluginAssembly.AssemblyLoadContext.Unload();
            pluginAssembly.RemoveAllEventHandlers(_force.Server);

            foreach (var plugin in pluginAssembly.Plugins)
            {
                try
                {
                    plugin.Value.OnPluginUnloadAsync(_force);
                }
                catch (NotImplementedException)
                {
                    // ignored
                }
            }
        }

        return removed;
    }

    public bool ReloadPlugins(PluginAssembly pluginAssembly)
    {
        bool unloaded = UnloadPlugins(pluginAssembly);

        if (unloaded)
        {
            UnloadPlugins(pluginAssembly);

            pluginAssembly = GetPluginsFromAssembly(pluginAssembly.Assembly.Location);
            LoadPlugins(pluginAssembly);
        }

        return unloaded;
    }

    public static string[] GetPluginFilesFromDirectory(DirectoryInfo directory, SearchOption searchOption = SearchOption.AllDirectories)
    {
        return Directory.GetFiles(directory.FullName, "*.dll", searchOption);
    }

    public static PluginAssembly GetPluginsFromAssembly(string assemblyPath)
    {
        var assemblyLoadContext = new AssemblyLoadContext(null, isCollectible: true);
        var assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

        return new PluginAssembly(assemblyLoadContext, assembly);
    }
}
