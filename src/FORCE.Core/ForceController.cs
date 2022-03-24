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
    }

    public void LoadPlugins()
    {
        string[] pluginFiles = PluginManager.GetPluginFilesFromDirectory(new DirectoryInfo("Plugins"));
        var pluginAssemblies = pluginFiles.Select(PluginManager.GetPluginAssemblyFromPath);
        pluginAssemblies.ForEach(p => PluginManager.LoadPluginAssembly(p));

        foreach (var plugin in PluginManager.PluginAssemblies.SelectMany(p => p.Plugins))
        {
            Console.WriteLine($"Loaded {plugin}");

            foreach (var command in plugin.Commands)
            {
                Console.WriteLine($"   {command}");
            }

            Console.WriteLine();
        }
    }
}
