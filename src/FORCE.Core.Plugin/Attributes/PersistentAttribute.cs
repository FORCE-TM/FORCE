namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that the value of a field or property should be preserved after a plugin reloads.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
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
