using FORCE.Core.Shared;

namespace FORCE.Core.Plugin.Models;

internal class CommandGroupInfo : ICommandGroupAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    public string[] Prefixes { get; set; } = null!;
    public string? Summary { get; set; }
    public PlayerRole? RequiredRole { get; set; }
    public bool HideIfUnauthorized { get; set; }
    public List<CommandInfo>? Commands { get; set; }
    public ClassInfo Class { get; set; } = null!;
}
