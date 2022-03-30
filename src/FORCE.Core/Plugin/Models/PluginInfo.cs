namespace FORCE.Core.Plugin.Models;

internal class PluginInfo : PluginDisplayInfo
{
    public List<CommandInfo> Commands { get; set; } = null!;
    public ClassInfo Class { get; set; } = null!;
}
