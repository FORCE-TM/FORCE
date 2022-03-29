using FORCE.Core.Models;
using FORCE.Core.Plugin;

namespace FORCE.Core.Builders;

internal class PluginBuilder : IPluginAttribute, ISummaryAttribute
{
    private readonly List<CommandBuilder> _commands;

    public string Name { get; } = null!;
    public Version Version { get; } = null!;
    public string? Author { get; }
    public string? Summary { get; }

    public PluginBuilder()
    {
        _commands = new List<CommandBuilder>();
    }

    public PluginInfo Build() => new()
    {

    };
}
