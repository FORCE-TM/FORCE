using FORCE.Core.Plugin;

namespace FORCE.Core.Models;

internal class PluginInfo : IPluginAttribute, ISummaryAttribute
{
    public string Name { get; } = null!;
    public Version Version { get; } = null!;
    public string? Author { get; }
    public string? Summary { get; }
    public List<CommandInfo>? Commands { get; set; }
}
