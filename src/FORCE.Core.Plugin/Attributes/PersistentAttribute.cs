namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that the value of a field or property should be preserved after a plugin reloads.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class PersistentAttribute : Attribute, IPersistentAttribute
{
    /// <summary>
    /// <inheritdoc cref="PersistentAttribute"/>
    /// </summary>
    public PersistentAttribute()
    {
        IsPersistent = true;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsPersistent { get; set; }
}

public interface IPersistentAttribute
{
    public bool IsPersistent { get; set; }
}
