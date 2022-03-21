using System.Runtime.Loader;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

internal class PluginManager
{
    public List<PluginAssembly> PluginAssemblies { get; } = new();

    internal readonly ForceController Force;

    public PluginManager(ForceController force)
    {
        Force = force;
    }

    public bool LoadPluginAssembly(PluginAssembly pluginAssembly)
    {
        if (pluginAssembly.Plugins.Count == 0)
            return false;

        PluginAssemblies.Add(pluginAssembly);

        foreach (var plugin in pluginAssembly.Plugins)
        {
            plugin.MainInstance.OnPluginLoadAsync();
        }

        return true;
    }

    public bool UnloadPluginAssembly(PluginAssembly pluginAssembly)
    {
        bool removed = PluginAssemblies.Remove(pluginAssembly);

        if (removed)
        {
            foreach (var plugin in pluginAssembly.Plugins)
            {
                try
                {
                    pluginAssembly.AssemblyLoadContext.Unload();

                    plugin.MainInstance.OnPluginUnloadAsync();

                    // After OnPluginUnloadAsync to make sure the plugin can not register new events
                    pluginAssembly.RemoveAllEventHandlers(Force.Server);
                }
                catch (NotImplementedException)
                {
                    // ignored
                }
            }
        }

        return removed;
    }

    public bool ReloadPluginAssembly(PluginAssembly pluginAssembly)
    {
        bool unloaded = UnloadPluginAssembly(pluginAssembly);

        if (unloaded)
        {
            UnloadPluginAssembly(pluginAssembly);

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
