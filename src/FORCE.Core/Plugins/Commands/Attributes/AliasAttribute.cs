namespace FORCE.Core.Plugins.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AliasAttribute : Attribute
{
    public string[] Names { get; }

    public AliasAttribute(params string[] names)
    {
        Names = names;
    }
}
