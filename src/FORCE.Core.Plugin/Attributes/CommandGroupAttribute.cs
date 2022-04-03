namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that a class is a command group. This is optional.<br/>
/// Command groups allow to add a prefix to all of the commands in a class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandGroupAttribute : Attribute, ICommandGroupAttribute
{
    /// <inheritdoc cref="CommandGroupAttribute"/>
    /// <param name="prefixes">Prefix of the command group. Aliases can be added.</param>
    public CommandGroupAttribute(params string[] prefixes)
    {
        Prefixes = prefixes;
    }

    /// <inheritdoc/>
    public string[] Prefixes { get; }
}

public interface ICommandGroupAttribute
{
    /// <summary>
    /// The prefix and aliases of the command group.
    /// </summary>
    public string[] Prefixes { get; }
}
