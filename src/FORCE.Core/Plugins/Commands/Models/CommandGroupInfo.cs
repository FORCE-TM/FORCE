using FORCE.Core.Enums;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandGroupInfo
{
    public string Prefix => Prefixes.First();
    public string[] Prefixes { get; private set; }
    public string Summary { get; private set; }
    public PlayerRole? RequiredRole { get; private set; }
    public bool HideUnauthorized { get; set; }

    private CommandGroupInfo()
    {
    }

    public static bool TryGetFromClass(Type commandClass, out CommandGroupInfo commandGroup)
    {
        if (!commandClass.TryGetCustomAttribute<CommandGroupAttribute>(out var commandGroupAttribute))
        {
            commandGroup = null;
            return false;
        }

        commandGroup = new()
        {
            Prefixes = commandGroupAttribute.Prefixes
        };

        if (commandClass.TryGetCustomAttribute<SummaryAttribute>(out var summaryAttribute))
        {
            commandGroup.Summary = summaryAttribute.Text;
        }

        if (commandClass.TryGetCustomAttribute<RequireRoleAttribute>(out var requireRoleAttribute))
        {
            commandGroup.RequiredRole = requireRoleAttribute.Role;
            commandGroup.HideUnauthorized = requireRoleAttribute.HideUnauthorized;
        }

        return true;
    }
}
