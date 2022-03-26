using FORCE.Core.Extensions;
using FORCE.Core.Plugins;
using FORCE.Core.Utilities;

namespace FORCE.Core;

public class ForceController
{
    public TmServer Server { get; }

    internal PluginManager PluginManager { get; }

    internal ObjectSemaphoreSlim ObjectSemaphore { get; }

    public ForceController(string serverHost = "127.0.0.1", int serverPort = 5000)
    {
        ObjectSemaphore = new ObjectSemaphoreSlim();

        Server = new TmServer(serverHost, serverPort, ObjectSemaphore);

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
