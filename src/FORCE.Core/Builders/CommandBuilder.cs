using FORCE.Core.Models;
using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Core.Builders;

internal class CommandBuilder : ICommandAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    private CommandGroupBuilder? _commandGroup;
    private List<CommandParameterBuilder>? _parameters;

    public string[] Names { get; }
    public string? Summary { get; private set; }
    public PlayerRole? RequiredRole { get; private set; }
    public bool? HideIfUnauthorized { get; private set; }

    public CommandBuilder(ICommandAttribute command)
    {
        Names = command.Names;
    }

    public CommandBuilder WithGroup(CommandGroupBuilder commandGroup)
    {
        _commandGroup = commandGroup;
        return this;
    }

    public CommandBuilder WithSummary(ISummaryAttribute summary)
    {
        Summary = summary.Summary;
        return this;
    }

    public CommandBuilder WithRequiredRole(IRequireRoleAttribute requireRole)
    {
        RequiredRole = requireRole.RequiredRole;
        HideIfUnauthorized = requireRole.HideIfUnauthorized;
        return this;
    }

    public CommandBuilder WithParameters(params CommandParameterBuilder[] parameters)
    {
        _parameters ??= new List<CommandParameterBuilder>();
        _parameters.AddRange(parameters);
        return this;
    }

    public CommandInfo Build() => new()
    {
        Names = Names,
        Summary = Summary,
        RequiredRole = RequiredRole,
        HideIfUnauthorized = HideIfUnauthorized,
        Group = _commandGroup?.Build(),
        Parameters = _parameters?.Select(p => p.Build()).ToList()
    };
}
