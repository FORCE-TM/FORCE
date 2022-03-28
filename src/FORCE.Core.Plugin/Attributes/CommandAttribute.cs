namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that a method is a plugin command.<br/>
/// This must be used for a command to be discoverable.<br/>
/// Unless this method is belongs to a <see cref="PluginBase"/> class, its parent class must inherit from <see cref="CommandBase"/>.<br/>
/// In order to add a group prefix to a command, its parent class must be decorated with <see cref="CommandGroupAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CommandAttribute : Attribute, ICommandAttribute
{
    /// <summary>
    /// <inheritdoc cref="CommandAttribute"/>
    /// </summary>
    /// <param name="names">Name of the command. Aliases can be added.</param>
    public CommandAttribute(params string[] names)
    {
        Names = names;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string[] Names { get; set; }
}

public interface ICommandAttribute
{
    /// <summary>
    /// The name and aliases of the command.
    /// </summary>
    public string[] Names { get; set; }
}
