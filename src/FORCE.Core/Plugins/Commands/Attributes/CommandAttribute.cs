namespace FORCE.Core.Plugins.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string[] Names { get; }

    public CommandAttribute(params string[] names)
    {
        Names = names;
    }
}
