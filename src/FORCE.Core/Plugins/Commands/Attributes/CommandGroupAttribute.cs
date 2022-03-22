namespace FORCE.Core.Plugins.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandGroupAttribute : Attribute
{
    public string[] Prefixes { get; }

    public CommandGroupAttribute(params string[] prefixes)
    {
        Prefixes = prefixes;
    }
}
