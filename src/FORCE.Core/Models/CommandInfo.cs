using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Core.Models;

internal class CommandInfo : ICommandAttribute, ISummaryAttribute, IRequireRoleAttribute
{
    public string[] Names { get; set; } = null!;
    public string? Summary { get; set; }
    public PlayerRole? RequiredRole { get; set; }
    public bool? HideIfUnauthorized { get; set; }
    public CommandGroupInfo? Group { get; set; }
    public List<CommandParameterInfo>? Parameters { get; set; }
}
