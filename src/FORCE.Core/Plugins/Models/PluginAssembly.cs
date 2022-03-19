using System.Reflection;
using System.Runtime.Loader;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;

namespace FORCE.Core.Plugins.Models;

internal class PluginAssembly
{
    public Dictionary<Plugin, IPlugin> Plugins { get; } = new();

    public AssemblyLoadContext AssemblyLoadContext { get; }
    public Assembly Assembly { get; }

    public PluginAssembly(AssemblyLoadContext assemblyLoadContext, Assembly assembly = null)
    {
        AssemblyLoadContext = assemblyLoadContext;
        Assembly = assembly ?? assemblyLoadContext.Assemblies.Single();

        foreach (var module in Assembly.Modules)
        {
            foreach (var pluginClass in module.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t)))
            {
                if (!pluginClass.TryGetCustomAttribute<PluginAttribute>(out var pluginAttribute))
                    throw new($"Plugin class must be decorated with a {nameof(PluginAttribute)}");

                var plugin = new Plugin(pluginAttribute.Name, pluginAttribute.Version, pluginAttribute.Author);

                var pluginInstance = (IPlugin)Activator.CreateInstance(pluginClass);

                Plugins.Add(plugin, pluginInstance);
            }
        }
    }

    public void RemoveAllEventHandlers(object instance, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    {
        var type = instance.GetType();

        foreach (var field in type.GetEvents(bindingFlags).Select(e => e.DeclaringType.GetField(e.Name, bindingFlags)))
        {
            var @event = type.GetEvent(field.Name, bindingFlags);

            if (@event == null)
                continue;

            if (field.GetValue(instance) is Delegate @delegate)
            {
                foreach (var handler in @delegate.GetInvocationList().Where(h => h.Method.Module.Assembly == Assembly))
                {
                    @event.RemoveEventHandler(instance, handler);
                }
            }
        }
    }
}
