using FORCE.Core.Plugin.Models;
using FORCE.Core.Shared;

namespace FORCE.Core.Plugin.Builders;

internal class CommandGroupBuilder : ICommandGroupAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    public string[] Prefixes { get; }
    public string? Summary { get; private set; }
    public PlayerRole? RequiredRole { get; private set; }
    public bool HideIfUnauthorized { get; private set; }

    public CommandGroupBuilder(ICommandGroupAttribute commandGroup)
    {
        foreach (string prefix in commandGroup.Prefixes)
            if (prefix.Contains(' '))
                throw new InvalidOperationException($"Command group prefix can not contain any space. Prefix: {prefix}");

        Prefixes = commandGroup.Prefixes;
    }

    public CommandGroupBuilder WithSummary(ISummaryAttribute summary)
    {
        Summary = summary.Summary;
        return this;
    }

    public CommandGroupBuilder WithRequireRole(IRequireRoleAttribute requireRole)
    {
        RequiredRole = requireRole.RequiredRole;
        HideIfUnauthorized = requireRole.HideIfUnauthorized;
        return this;
    }

    public CommandGroupInfo Build() => new()
    {
        Prefixes = Prefixes,
        Summary = Summary,
        RequiredRole = RequiredRole,
        HideIfUnauthorized = HideIfUnauthorized
    };
}
