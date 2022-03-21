using System.Reflection;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;
using FORCE.Core.Plugins.Commands.Models;

namespace FORCE.Core.Plugins.Commands;

internal class CommandListBuilder
{
    private static readonly Type CommandBaseType = typeof(ForceCommand);

    private readonly PluginBuilder _pluginBuilder;

    public CommandListBuilder(PluginBuilder pluginBuilder)
    {
        _pluginBuilder = pluginBuilder;
    }

    private IEnumerable<CommandInfo> ResolveCommandsFromGroupClasses(Module module)
    {
        foreach (var commandClass in module.GetTypes().Where(IsValidCommandGroupClass))
        {
            if (!commandClass.TryGetCustomAttribute<CommandGroupAttribute>(out var commandGroupAttribute))
                continue;

            commandClass.TryGetCustomAttribute<AliasAttribute>(out var commandGroupAliasAttribute);
            commandClass.TryGetCustomAttribute<SummaryAttribute>(out var commandGroupSummary);

            var group = new CommandGroupInfo(commandGroupAttribute, commandGroupAliasAttribute, commandGroupSummary);

            foreach (var command in ResolveCommandsFromMethods(commandClass))
            {
                command.SetGroup(group);

                yield return command;
            }
        }
    }

    private IEnumerable<CommandInfo> ResolveCommandsFromMethods(Type @class)
    {
        foreach (var method in @class.GetMethods().Where(IsValidCommandMethod))
        {
            if (!method.TryGetCustomAttribute<CommandAttribute>(out var commandAttribute))
                continue;

            method.TryGetCustomAttribute<AliasAttribute>(out var commandAliasAttribute);
            method.TryGetCustomAttribute<SummaryAttribute>(out var commandSummaryAttribute);

            var command = new CommandInfo(commandAttribute, commandAliasAttribute, commandSummaryAttribute);

            yield return command;
        }
    }

    // (without attribute checks)
    private bool IsValidCommandGroupClass(Type type)
        => CommandBaseType.IsAssignableFrom(type) &&
           type.IsClass &&
           type.IsPublic &&
           !type.IsAbstract &&
           !type.ContainsGenericParameters;

    // (without attribute checks)
    private bool IsValidCommandMethod(MethodBase method)
        => method.IsPublic &&
           !method.ContainsGenericParameters;

    public IEnumerable<CommandInfo> Build()
        => ResolveCommandsFromGroupClasses(_pluginBuilder.Module)
            .Concat(ResolveCommandsFromMethods(_pluginBuilder.PluginClass));
}
