namespace FORCE.Core.Plugins.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PersistentAttribute : Attribute
{
    public bool BetweenReload { get; }

    public PersistentAttribute(bool betweenReload = false)
    {
        BetweenReload = betweenReload;
    }
}
