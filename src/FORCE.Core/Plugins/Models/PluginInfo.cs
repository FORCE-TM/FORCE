using System.Text;
using FORCE.Core.Plugins.Commands.Models;

namespace FORCE.Core.Plugins.Models;

public class PluginInfo
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Author { get; set; }
    public string Summary { get; set; }

    internal List<CommandInfo> Commands { get; set; }

    internal ForcePlugin MainInstance { get; set; }

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
}
