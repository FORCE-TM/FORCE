using System.Reflection;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandInfo
{
    public string Name { get; private set; }
    public string[] Aliases { get; private set; }
    public string[] AllNames { get; private set; }
    public bool Admin { get; private set; }
    public string Summary { get; private set; }

    public List<CommandParameterInfo> Parameters { get; private set; }

    public CommandGroupInfo Group { get; private set; }
    public bool BelongsToGroup { get; private set; }

    private CommandInfo()
    {
    }

    public static bool TryGetFromMethod(MethodInfo method, out CommandInfo command)
    {
        command = new();

        if (!method.TryGetCustomAttribute<CommandAttribute>(out var commandAttribute))
            return false;

        method.TryGetCustomAttribute<AliasAttribute>(out var aliasAttribute);
        method.TryGetCustomAttribute<SummaryAttribute>(out var summaryAttribute);

        command.Name = commandAttribute.Name;
        command.Aliases = aliasAttribute?.Names ?? Array.Empty<string>();
        command.AllNames = new[] { command.Name }.Concat(command.Aliases).ToArray();
        command.Admin = commandAttribute.Admin;
        command.Summary = summaryAttribute?.Text;

        command.Parameters = new List<CommandParameterInfo>();
            
        foreach (var parameterInfo in method.GetParameters())
        {
            var commandParameter = new CommandParameterInfo(parameterInfo);

            command.Parameters.Add(commandParameter);
        }

        return true;
    }

    public void SetGroup(CommandGroupInfo group)
    {
        Group = group;
        BelongsToGroup = group != null;
    }
}
