using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandGroupInfo
{
    public string Prefix { get; private set; }
    public string[] Aliases { get; private set; }
    public string[] AllNames { get; private set; }
    public bool Admin { get; private set; }
    public string Summary { get; private set; }


    private CommandGroupInfo()
    {
    }

    public static bool TryGetFromClass(Type commandClass, out CommandGroupInfo commandGroup)
    {
        commandGroup = new();

        if (!commandClass.TryGetCustomAttribute<CommandGroupAttribute>(out var commandGroupAttribute))
            return false;

        commandClass.TryGetCustomAttribute<AliasAttribute>(out var aliasAttribute);
        commandClass.TryGetCustomAttribute<SummaryAttribute>(out var summaryAttribute);

        commandGroup.Prefix = commandGroupAttribute.Prefix;
        commandGroup.Aliases = aliasAttribute?.Names ?? Array.Empty<string>();
        commandGroup.AllNames = new[] { commandGroup.Prefix }.Concat(commandGroup.Aliases).ToArray();
        commandGroup.Admin = commandGroupAttribute.Admin;
        commandGroup.Summary = summaryAttribute?.Text;

        return true;
    }
}
