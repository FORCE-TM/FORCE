using FORCE.Core.Models;
using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Core.Builders;

internal class CommandGroupBuilder : ICommandGroupAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    public string[]? GroupPrefixes { get; }
    public string? Summary { get; private set; }
    public PlayerRole? RequiredRole { get; private set; }
    public bool? HideIfUnauthorized { get; private set; }

    public CommandGroupBuilder(ICommandGroupAttribute commandGroup)
    {
        foreach (string groupPrefix in commandGroup.GroupPrefixes!)
            if (groupPrefix.Contains(' '))
                throw new InvalidOperationException($"Command group prefix can not contain any space. Prefix: {groupPrefix}");

        GroupPrefixes = commandGroup.GroupPrefixes;
    }

    public CommandGroupBuilder WithSummary(ISummaryAttribute summary)
    {
        Summary = summary.Summary;
        return this;
    }

    public CommandGroupBuilder WithRequiredRole(IRequireRoleAttribute requireRole)
    {
        RequiredRole = requireRole.RequiredRole;
        HideIfUnauthorized = requireRole.HideIfUnauthorized;
        return this;
    }

    public CommandGroupInfo Build() => new()
    {
        GroupPrefixes = GroupPrefixes,
        Summary = Summary,
        RequiredRole = RequiredRole,
        HideIfUnauthorized = HideIfUnauthorized
    };
}
