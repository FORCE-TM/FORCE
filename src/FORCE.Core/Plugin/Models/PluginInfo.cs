using System.Reflection;
using System.Runtime.Loader;

namespace FORCE.Core.Plugin.Models;

internal class PluginInfo : PluginDisplayInfo
{
    public List<CommandInfo> Commands { get; set; } = null!;
    public ClassInfo Class { get; set; } = null!;
    public AssemblyLoadContext AssemblyLoadContext { get; set; } = null!;
    public Assembly Assembly => AssemblyLoadContext.Assemblies.Single();

    public void SetContext(ContextBase context)
    {
        var pluginInstance = Class.GetInstance<PluginBase>();

        pluginInstance.Plugin = context.Plugin;
        pluginInstance.Server = context.Server;
        pluginInstance.ColorScheme = context.ColorScheme;
    }
}
