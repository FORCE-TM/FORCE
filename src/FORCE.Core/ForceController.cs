using FORCE.Core.Extensions;
using FORCE.Core.Plugins;

namespace FORCE.Core;

public class ForceController
{
    public TmServer Server { get; }

    internal PluginManager PluginManager { get; }

    public ForceController(string serverHost = "127.0.0.1", int serverPort = 5000)
    {
        Server = new TmServer(serverHost, serverPort);

        PluginManager = new(this);

        string[] pluginFiles = PluginManager.GetPluginFilesFromDirectory(new DirectoryInfo("Plugins"));

        var pluginAssembles = pluginFiles.Select(PluginManager.GetPluginsFromAssembly);

        pluginAssembles.ForEach(p => PluginManager.LoadPlugins(p));

        foreach (var plugin in PluginManager.PluginAssemblies.SelectMany(p => p.Plugins.Keys))
        {
            Console.WriteLine($"Loaded {plugin.Name} v{plugin.Version} by {plugin.Author}");
        }
    }
}
