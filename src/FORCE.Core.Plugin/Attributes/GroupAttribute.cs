namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that a class is a command group.<br/>
/// Command groups allow to add a prefix to all of the commands in a class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class GroupAttribute : Attribute, IGroupAttribute
{
    /// <summary>
    /// <inheritdoc cref="GroupAttribute"/>
    /// </summary>
    /// <param name="prefixes">Prefix of the command group. Aliases can be added.</param>
    public GroupAttribute(params string[] prefixes)
    {
        Prefixes = prefixes;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string[] Prefixes { get; set; }
}

public interface IGroupAttribute
{
    /// <summary>
    /// The prefix and aliases of the command group.
    /// </summary>
    public string[] Prefixes { get; set; }
}
