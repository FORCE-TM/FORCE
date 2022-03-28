using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that one command or all commands from a class require a player to have a role in order to use them.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizeAttribute
{
    /// <summary>
    /// <inheritdoc cref="AuthorizeAttribute"/>
    /// </summary>
    /// <param name="role">Role from which a player will be able to use a command.</param>
    /// <param name="hideIfUnauthorized">Whether or not to hide the existence of a command to unauthorized players.</param>
    public AuthorizeAttribute(PlayerRole role, bool hideIfUnauthorized = true)
    {
        Role = role;
        HideIfUnauthorized = hideIfUnauthorized;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public PlayerRole Role { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
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
