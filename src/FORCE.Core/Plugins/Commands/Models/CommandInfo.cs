using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandInfo
{
    public string Name { get; }
    public string[] Aliases { get; }
    public bool Admin { get; }
    public string Summary { get; }

    public string[] AllNames { get; }

    public CommandGroupInfo Group { get; private set; }
    public bool BelongsToGroup { get; private set; }

    public CommandInfo(CommandAttribute command, AliasAttribute alias, SummaryAttribute summary)
    {
        Name = command.Name;
        Aliases = alias?.Names ?? Array.Empty<string>();
        Admin = command.Admin;
        Summary = summary?.Text;

        AllNames = new[] { Name }.Concat(Aliases).ToArray();
    }

    public void SetGroup(CommandGroupInfo group)
    {
        Group = group;
        BelongsToGroup = true;
    }
}
