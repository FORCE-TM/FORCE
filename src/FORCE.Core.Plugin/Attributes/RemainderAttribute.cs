namespace FORCE.Core.Plugin;

/// <summary>
/// Indicates that starting from a parameter, the command arguments should not be split anymore.<br/>
/// The parameter must be of type <see cref="string"/>, and there can not be more parameters after it.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class RemainderAttribute : Attribute, IRemainderAttribute
{
    /// <inheritdoc cref="RemainderAttribute"/>
    public RemainderAttribute()
    {
        IsRemainder = true;
    }

    /// <inheritdoc/>
    public bool IsRemainder { get; set; }
}

public interface IRemainderAttribute
{
    public bool IsRemainder { get; set; }
}
