using System.Runtime.Loader;
using FORCE.Core.Plugin.Builders;
using FORCE.Core.Plugin.Models;

namespace FORCE.Core.Plugin;

internal class PluginManager
{
    public event Action<PluginInfo, bool>? OnPluginLoaded;
    public event Action<PluginInfo, bool>? OnPluginUnloaded;

    private readonly Force _force;
    private readonly List<PluginInfo> _plugins;

    public PluginManager(Force force)
    {
        _force = force;
        _plugins = new List<PluginInfo>();
    }

    public List<PluginInfo> GetLoadedPlugins()
        => _plugins;

    public void LoadPlugin(PluginInfo plugin, bool reload = false)
    {
        _plugins.Add(plugin);

        plugin.Class.GetInstance<PluginBase>().OnPluginLoadAsync(reload);

        OnPluginLoaded?.Invoke(plugin, reload);
    }

    public bool UnloadPlugin(PluginInfo plugin, bool reload = false)
    {
        if (!_plugins.Remove(plugin))
            return false;

        plugin.AssemblyLoadContext.Unload();
        plugin.Class.GetInstance<PluginBase>().OnPluginUnloadAsync(reload);

        // TODO: Remove subscribed event handlers
        // Only after OnPluginUnload, to make sure the plugin can not register any new event

        OnPluginUnloaded?.Invoke(plugin, reload);

        return true;
    }

    public bool ReloadPlugin(PluginInfo plugin)
    {
        if (!UnloadPlugin(plugin, true))
            return false;

        if (!TryBuildPluginFromAssemblyPath(plugin.Assembly.Location, out var reloadedPlugin))
            return false;

        var oldPersistentMembers = plugin.Commands
            .Select(c => c.Class)
            .Where(c => c.Instanced)
            .ToDictionary(c => c.Type.FullName!, c => c.GetPersistentMembers());

        foreach (var commandClass in reloadedPlugin.Commands.Select(c => c.Class))
            if (oldPersistentMembers.TryGetValue(commandClass.Type.FullName!, out var persistentMembers))
                commandClass.SetPersistentMembers(persistentMembers);

        LoadPlugin(reloadedPlugin, true);
        return true;
    }

    public bool TryBuildPluginFromAssemblyPath(string assemblyPath, out PluginInfo plugin)
    {
        var assemblyLoadContext = new AssemblyLoadContext(null, true);
        var assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        var module = assembly.Modules.SingleOrDefault(PluginBuilder.IsValidPluginModule);

        if (module == null)
        {
            plugin = null!;
            return false;
        }

        var pluginBuilder = new PluginBuilder(module, _force);
        plugin = pluginBuilder.Build();

        plugin.AssemblyLoadContext = assemblyLoadContext;

        return true;
    }
}
