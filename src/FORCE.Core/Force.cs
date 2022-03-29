using System.Runtime.Loader;
using System.Text.Json;
using FORCE.Core.Models;
using FORCE.Core.Plugin.Builders;
using FORCE.Core.Shared;

namespace FORCE.Core;

public class Force
{
    public ForceSettings Settings { get; }
    public TmServer TmServer { get; }

    public Force()
    {
        Settings = JsonSerializer.Deserialize<ForceSettings>(File.ReadAllText(Path.Combine("Files", "Settings.json")))!;

        TmServer = new TmServer(Settings.Server.Host, Settings.Server.Port);

        var assemblyLoadContext = new AssemblyLoadContext(null, isCollectible: true);
        var assembly = assemblyLoadContext.LoadFromAssemblyPath(@"C:\Users\furti\source\FORCE-TM\FORCE\src\Plugins\FORCE.Plugin.AutoGreeter\bin\Debug\net6.0\FORCE.Plugin.AutoGreeter.dll");

        var pluginBuilder = new PluginBuilder(assembly.Modules.Single());

        var plugin = pluginBuilder.Build();

        Console.WriteLine($"Loaded {plugin}");

        foreach (var command in plugin.Commands)
        {
            Console.WriteLine($"   {command}");
        }
    }
}
