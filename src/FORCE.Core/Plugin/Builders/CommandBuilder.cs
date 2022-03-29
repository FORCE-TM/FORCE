using FORCE.Core.Plugin.Models;
using FORCE.Core.Shared;

namespace FORCE.Core.Plugin.Builders;

internal class CommandBuilder : ICommandAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    private readonly List<CommandParameterInfo> _parameters;
    private CommandGroupInfo? _commandGroup;

    public string[] Names { get; }
    public string? Summary { get; private set; }
    public PlayerRole? RequiredRole { get; private set; }
    public bool? HideIfUnauthorized { get; private set; }

    public CommandBuilder(ICommandAttribute command)
    {
        Names = command.Names;
        _parameters = new List<CommandParameterInfo>();
    }

    public CommandBuilder WithGroup(CommandGroupInfo commandGroup)
    {
        _commandGroup = commandGroup;
        return this;
    }

    public CommandBuilder WithSummary(ISummaryAttribute summary)
    {
        Summary = summary.Summary;
        return this;
    }

    public CommandBuilder WithRequireRole(IRequireRoleAttribute requireRole)
    {
        RequiredRole = requireRole.RequiredRole;
        HideIfUnauthorized = requireRole.HideIfUnauthorized;
        return this;
    }

    public CommandBuilder WithParameters(IEnumerable<CommandParameterInfo> parameters)
    {
        _parameters.AddRange(parameters);
        return this;
    }

    public CommandInfo Build() => new()
    {
        Names = Names,
        Summary = Summary,
        RequiredRole = RequiredRole,
        HideIfUnauthorized = HideIfUnauthorized,
        Group = _commandGroup,
        Parameters = _parameters
    };
}
