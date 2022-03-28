using System.Text;
using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Core.Models;

internal class PluginModel : IPluginAttribute, IColoredString
{
    public string Name { get; set; }
    public Version Version { get; set; }
    public string? Author { get; set; }

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
