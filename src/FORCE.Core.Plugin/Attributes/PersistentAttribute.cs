namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that the value of a field or property should be preserved after a plugin reloads.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class PersistentAttribute : Attribute, IPersistentAttribute
{
    /// <inheritdoc/>
    public PersistentAttribute()
    {
        IsPersistent = true;
    }

    /// <inheritdoc/>
    public bool IsPersistent { get; }
}

public interface IPersistentAttribute
{
    public bool IsPersistent { get; }
}
