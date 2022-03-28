using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that one command or all commands from a class require a player to have a role in order to use them.<br/>
/// By default, a command can be used by anyone without requiring a role.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizeAttribute : Attribute, IAuthorizeAttribute
{
    /// <inheritdoc cref="AuthorizeAttribute"/>
    /// <param name="role">Role from which a player will be able to use a command.</param>
    /// <param name="hideIfUnauthorized">Whether or not to hide the existence of a command to unauthorized players.</param>
    public AuthorizeAttribute(PlayerRole role, bool hideIfUnauthorized = true)
    {
        Role = role;
        HideIfUnauthorized = hideIfUnauthorized;
    }

    /// <inheritdoc/>
    public PlayerRole Role { get; set; }

    /// <inheritdoc/>
    public bool HideIfUnauthorized { get; set; }
}

public interface IAuthorizeAttribute
{
    /// <summary>
    /// The role from which a player will be able to use a command.
    /// </summary>
    public PlayerRole Role { get; set; }

    /// <summary>
    /// Whether or not to hide the existence of a command to unauthorized players.
    /// </summary>
    public bool HideIfUnauthorized { get; set; }
}
