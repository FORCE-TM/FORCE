using FORCE.Core.Plugins.Commands.Models;

namespace FORCE.Core.Plugins.Models;

internal class PluginInfo
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Author { get; set; }
    public string Summary { get; set; }

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

    public List<CommandInfo> Commands { get; set; }
}
