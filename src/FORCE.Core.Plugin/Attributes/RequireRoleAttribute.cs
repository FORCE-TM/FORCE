using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that one command or all commands from a class require a player to have a role in order to use them.<br/>
/// By default, a command can be used by anyone without requiring a role.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequireRoleAttribute : Attribute, IRequireRoleAttribute
{
    /// <inheritdoc cref="RequireRoleAttribute"/>
    /// <param name="role">Role from which a player will be able to use a command.</param>
    /// <param name="hideIfUnauthorized">Whether or not to hide the existence of a command to unauthorized players.</param>
    public RequireRoleAttribute(PlayerRole role, bool hideIfUnauthorized = true)
    {
        RequiredRole = role;
        HideIfUnauthorized = hideIfUnauthorized;
    }

    /// <inheritdoc/>
    public PlayerRole? RequiredRole { get; set; }

    /// <inheritdoc/>
    public bool? HideIfUnauthorized { get; set; }
}

public interface IRequireRoleAttribute
{
    /// <summary>
    /// The role from which a player will be able to use a command.
    /// </summary>
    public PlayerRole? RequiredRole { get; set; }

    /// <summary>
    /// Whether or not to hide the existence of a command to unauthorized players.
    /// </summary>
    public bool? HideIfUnauthorized { get; set; }
}
