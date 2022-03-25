﻿using System.Reflection;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands;
using FORCE.Core.Plugins.Commands.Attributes;
using FORCE.Core.Plugins.Commands.Models;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins;

internal class PluginBuilder
{
    public static readonly Type PluginBaseType = typeof(ForcePlugin);

    public PluginManager PluginManager { get; }
    public Type PluginClass { get; }

    private readonly PluginAttribute _pluginAttribute;
    private readonly SummaryAttribute _summaryAttribute;

    public PluginBuilder(Module module, PluginManager pluginManager)
    {
        PluginManager = pluginManager;

        PluginClass = module.GetTypes().SingleOrDefault(IsValidPluginClass);

        if (PluginClass == null)
            throw new("None, or more than one plugin class were found in this module.");

        if (!PluginClass.TryGetCustomAttribute(out _pluginAttribute))
            throw new($"{PluginClass.FullName} class must be decorated with {nameof(PluginAttribute)}.");

        if (PluginClass.TryGetCustomAttribute<CommandGroupAttribute>(out _))
            throw new($"{PluginClass.FullName} class can not be decorated with both {nameof(PluginAttribute)} and {nameof(CommandGroupAttribute)}.");

        _summaryAttribute = PluginClass.GetCustomAttribute<SummaryAttribute>();
    }

    private IEnumerable<CommandInfo> GetCommands(PluginInfo plugin)
        => new CommandListBuilder(this, plugin).Build();

    // (without attribute checks)
    private bool IsValidPluginClass(Type type)
        => PluginBaseType.IsAssignableFrom(type) &&
           !CommandListBuilder.CommandBaseType.IsAssignableFrom(type) &&
           type.IsClass &&
           type.IsPublic &&
           !type.IsAbstract &&
           !type.ContainsGenericParameters;

    private ForcePlugin CreateInstance(PluginInfo plugin)
    {
        var instance = (ForcePlugin)Activator.CreateInstance(PluginClass);

        instance.Force = PluginManager.Force;
        instance.Plugin = plugin;

        return instance;
    }

    public PluginInfo Build()
    {
        var plugin = new PluginInfo()
        {
            Name = _pluginAttribute.Name,
            Version = _pluginAttribute.Version,
            Author = _pluginAttribute.Author,
            Summary = _summaryAttribute.Text
        };

        plugin.Commands = GetCommands(plugin).ToList();
        plugin.MainInstance = CreateInstance(plugin);

        return plugin;
    }
}
