using System.Text;
using FORCE.Core.Plugins.Commands.Models;

namespace FORCE.Core.Plugins.Models;

internal class PluginInfo
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Author { get; set; }
    public string Summary { get; set; }

    public List<CommandInfo> Commands { get; set; }

    public Type Class { get; set; }

    public ForcePlugin MainInstance { get; private set; }

    private Func<ForcePlugin> _newInstanceFunc;
    public Func<ForcePlugin> NewInstanceFunc
    {
        get => _newInstanceFunc;
        set
        {
            _newInstanceFunc = value;
            MainInstance = value.Invoke();
        }
    }

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
