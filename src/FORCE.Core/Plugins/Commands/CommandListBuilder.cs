using System.Reflection;
using FORCE.Core.Plugins.Commands.Models;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins.Commands;

internal class CommandListBuilder
{
    public static readonly Type CommandBaseType = typeof(CommandContext);

    private readonly PluginBuilder _pluginBuilder;
    private readonly PluginInfo _plugin;

    public CommandListBuilder(PluginBuilder pluginBuilder, PluginInfo plugin)
    {
        _pluginBuilder = pluginBuilder;
        _plugin = plugin;
    }

    private IEnumerable<CommandInfo> DiscoverCommandsFromModuleClasses(Module module)
    {
        foreach (var commandClass in module.GetTypes().Where(IsValidCommandGroupClass))
        {
            CommandGroupInfo.TryGetFromClass(commandClass, out var commandGroup);

            foreach (var command in DiscoverCommandsFromClassMethods(commandClass))
            {
                command.SetGroup(commandGroup);

                yield return command;
            }
        }
    }

    private IEnumerable<CommandInfo> DiscoverCommandsFromClassMethods(Type @class)
    {
        foreach (var method in @class.GetMethods().Where(IsValidCommandMethod))
        {
            if (CommandInfo.TryGetFromMethod(method, _plugin, out var command))
            {
                yield return command;
            }
        }
    }

    private bool IsValidCommandGroupClass(Type type)
        => CommandBaseType.IsAssignableFrom(type) &&
           type.IsClass &&
           type.IsPublic &&
           !type.IsAbstract &&
           !type.ContainsGenericParameters;

    private bool IsValidCommandMethod(MethodInfo method)
        => method.IsPublic &&
           method.ReturnType == typeof(Task) &&
           !method.ContainsGenericParameters;

    public IEnumerable<CommandInfo> Build()
        => DiscoverCommandsFromModuleClasses(_pluginBuilder.PluginClass.Module)
            .Concat(DiscoverCommandsFromClassMethods(_pluginBuilder.PluginClass));
}
