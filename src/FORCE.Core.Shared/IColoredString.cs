namespace FORCE.Core.Shared;

/// <summary>
/// Forces an object to implement a <see cref="ToColoredString"/> method, which is then used to return a TrackMania-colored version string of it.
/// </summary>
public interface IColoredString
{
    // This is only here to add a warning if the class does not override the ToString method
    public string ToString();

    /// <returns>A TrackMania-colored version of this <see cref="object.ToString"/>, using the provided <see cref="ColorScheme"/>.</returns>
    public string ToColoredString(ColorScheme scheme);
}
