﻿using System.Reflection;
using System.Text;
using FORCE.Core.Shared;

namespace FORCE.Core.Plugin.Models;

internal class CommandInfo : ICommandAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    public string[] Names { get; set; } = null!;
    public string? Summary { get; set; }
    public PlayerRole? RequiredRole { get; set; }
    public bool HideIfUnauthorized { get; set; }
    public CommandGroupInfo? Group { get; set; }
    public bool IsInGroup => Group != null;
    public List<CommandParameterInfo> Parameters { get; set; } = null!;
    public PluginInfo Plugin { get; set; } = null!;
    public ClassInfo Class { get; set; } = null!;
    public MethodInfo Method { get; set; } = null!;

    private bool _disabled;
    public bool Disabled
    {
        get => _disabled || (IsInGroup && Group!.Disabled);
        set => _disabled = value;
    }

    public override string ToString()
    {
        var commandBuilder = new StringBuilder();

        commandBuilder.Append('/');

        if (IsInGroup)
        {
            commandBuilder.Append(Group!.Prefixes.First());
            commandBuilder.Append(' ');
        }

        commandBuilder.Append(Names.First());

        foreach (var parameter in Parameters)
        {
            commandBuilder.Append(' ');
            commandBuilder.Append(parameter.HasDefaultValue ? '[' : '<');
            commandBuilder.Append(parameter.UsageName ?? parameter.Name);

            if (parameter.HasDefaultValue && parameter.DefaultValue is not null)
            {
                commandBuilder.Append('=');

                if (parameter.DefaultValue is string)
                {
                    commandBuilder.Append('"');
                    commandBuilder.Append(parameter.DefaultValue);
                    commandBuilder.Append('"');
                }
                else
                {
                    commandBuilder.Append(parameter.DefaultValue);
                }
            }
            else if (parameter.IsRemainder)
            {
                commandBuilder.Append("...");
            }

            commandBuilder.Append(parameter.HasDefaultValue ? ']' : '>');
        }

        return commandBuilder.ToString();
    }
}
