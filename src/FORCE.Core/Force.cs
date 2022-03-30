﻿using System.Text.Json;
using FORCE.Core.Models;
using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Core;

public class Force
{
    public ForceSettings Settings { get; }
    public TmServer Server { get; }

    internal PluginManager PluginManager { get; }

    public Force()
    {
        Settings = JsonSerializer.Deserialize<ForceSettings>(File.ReadAllText(Path.Combine("Files", "Settings.json")),
            new JsonSerializerOptions() { ReadCommentHandling = JsonCommentHandling.Skip })!;

        Server = new TmServer(Settings.Server.Host, Settings.Server.Port);

        PluginManager = new PluginManager(this);
    }

    public void LoadPlugins()
    {
        string pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), Settings.Plugins.Directory);

        foreach (string pluginFile in Settings.Plugins.Enabled)
        {
            string pluginPath = Path.Combine(pluginsDirectory, pluginFile);

            if (PluginManager.TryBuildPluginFromAssemblyPath(pluginPath, out var plugin))
            {
                PluginManager.LoadPlugin(plugin);

                Console.WriteLine($"Loaded {plugin}");

                foreach (var command in plugin.Commands)
                {
                    Console.WriteLine($"   {command}");
                }

                Console.WriteLine();
            }
        }
    }
}
