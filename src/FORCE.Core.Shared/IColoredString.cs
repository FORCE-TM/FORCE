namespace FORCE.Core.Shared;

public interface IColoredString
{
    // Adds a warning if the class does not override the ToString method
    public string ToString();

    /// <returns>A TrackMania-colored version of this <see cref="object.ToString"/></returns>
    public string ToColoredString(ColorScheme scheme);
}
