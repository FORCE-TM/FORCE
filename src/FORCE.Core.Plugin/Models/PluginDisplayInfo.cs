using System.Text;
using FORCE.Core.Shared;

namespace FORCE.Core.Plugin;

/// <summary>
/// A class containing primary information about a plugin, such as its <see cref="Name"/>, <see cref="Version"/>, <see cref="Author"/> or <see cref="Summary"/>.
/// </summary>
public sealed class PluginDisplayInfo : IPluginAttribute, ISummaryAttribute, IColoredString
{
    /// <inheritdoc/>
    public string Name { get; set; }

    /// <inheritdoc/>
    public Version Version { get; set; }

    /// <inheritdoc/>
    public string? Author { get; set; }

    /// <inheritdoc/>
    public string? Summary { get; set; }

    public override string ToString()
    {
        var nameBuilder = new StringBuilder();

        nameBuilder.Append(Name + " v" + Version);

        if (!string.IsNullOrWhiteSpace(Author))
        {
            nameBuilder.Append(" by " + Author);
        }

        return nameBuilder.ToString();
    }

    public string ToColoredString(ColorScheme scheme)
    {
        var sb = new StringBuilder();

        sb.Append(scheme.AccentColor + Name);
        sb.Append(scheme.BaseColor + " v");
        sb.Append(scheme.AccentColor + Version);

        if (!string.IsNullOrWhiteSpace(Author))
        {
            sb.Append(scheme.BaseColor + " by ");
            sb.Append(scheme.AccentColor + Author);
        }

        return sb.ToString();
    }
}
