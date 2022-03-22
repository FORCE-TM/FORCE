using System.Reflection;
using FORCE.Core.Enums;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandInfo
{
    public string Name => Names.First();
    public string[] Names { get; private set; }
    public string Summary { get; private set; }
    public PlayerRole? RequiredRole { get; private set; }
    public bool HideUnauthorized { get; set; }
    public List<CommandParameterInfo> Parameters { get; private set; }
    public CommandGroupInfo Group { get; private set; }
    public bool IsInGroup { get; private set; }

    private CommandInfo()
    {
    }

    public static bool TryGetFromMethod(MethodInfo method, out CommandInfo command)
    {
        if (!method.TryGetCustomAttribute<CommandAttribute>(out var commandAttribute))
        {
            command = null;
            return false;
        }

        command = new()
        {
            Names = commandAttribute.Names
        };

        if (method.TryGetCustomAttribute<SummaryAttribute>(out var summaryAttribute))
        {
            command.Summary = summaryAttribute.Text;
        }

        if (method.TryGetCustomAttribute<RequireRoleAttribute>(out var requireRoleAttribute))
        {
            command.RequiredRole = requireRoleAttribute.Role;
            command.HideUnauthorized = requireRoleAttribute.HideUnauthorized;
        }

        command.Parameters = new();

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
        IsInGroup = group != null;
    }
}
