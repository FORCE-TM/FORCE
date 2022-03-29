using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Core.Models;

internal class CommandGroupInfo : ICommandGroupAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    public string[]? GroupPrefixes { get; set; }
    public string? Summary { get; set; }
    public PlayerRole? RequiredRole { get; set; }
    public bool? HideIfUnauthorized { get; set; }
}
