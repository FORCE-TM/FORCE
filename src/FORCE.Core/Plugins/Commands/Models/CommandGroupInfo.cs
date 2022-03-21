using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandGroupInfo
{
    public string Prefix { get; }
    public string[] Aliases { get; }
    public bool Admin { get; }
    public string Summary { get; }

    public string[] AllNames { get; }

    public CommandGroupInfo(CommandGroupAttribute commandGroup, AliasAttribute alias, SummaryAttribute summary)
    {
        Prefix = commandGroup.Prefix;
        Aliases = alias?.Names ?? Array.Empty<string>();
        Admin = commandGroup.Admin;
        Summary = summary?.Text;

        AllNames = new[] { Prefix }.Concat(Aliases).ToArray();
    }
}
