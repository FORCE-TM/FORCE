using FORCE.Core.Enums;

namespace FORCE.Core.Plugins.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRoleAttribute : Attribute
{
    public PlayerRole Role { get; }
    public bool HideUnauthorized { get; }

    public RequireRoleAttribute(PlayerRole role = PlayerRole.None, bool hideUnauthorized = false)
    {
        Role = role;
        HideUnauthorized = hideUnauthorized;
    }
}
