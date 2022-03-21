using System.Reflection;
using System.Runtime.Loader;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

internal class PluginAssembly
{
    public List<PluginInfo> Plugins { get; } = new();

    public PluginManager PluginManager { get; }
    public AssemblyLoadContext AssemblyLoadContext { get; }
    public Assembly Assembly { get; }

    public PluginAssembly(PluginManager pluginManager, AssemblyLoadContext assemblyLoadContext, Assembly assembly = null)
    {
        PluginManager = pluginManager;
        AssemblyLoadContext = assemblyLoadContext;
        Assembly = assembly ?? assemblyLoadContext.Assemblies.Single();

        foreach (var module in Assembly.Modules)
        {
            try
            {
                var plugin = new PluginBuilder(module, PluginManager).Build();

                Plugins.Add(plugin);
            }
            catch
            {
                // ignored
            }
        }
    }

    public void RemoveAllEventHandlers(object eventClass, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    {
        var type = eventClass.GetType();

        foreach (var field in type.GetEvents(bindingFlags).Select(e => e.DeclaringType.GetField(e.Name, bindingFlags)))
        {
            var @event = type.GetEvent(field.Name, bindingFlags);

            if (@event == null)
                continue;

            if (field.GetValue(eventClass) is Delegate @delegate)
            {
                foreach (var handler in @delegate.GetInvocationList().Where(h => h.Method.Module.Assembly == Assembly))
                {
                    @event.RemoveEventHandler(eventClass, handler);
                }
            }
        }
    }
}
