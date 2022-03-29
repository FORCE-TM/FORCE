using System.Reflection;
using FORCE.Core.Extensions;
using FORCE.Core.Plugin.Models;

namespace FORCE.Core.Plugin.Builders;

// Heavily inspired by Discord.Net: https://github.com/discord-net/Discord.Net/blob/73399459eacbc15187edf4dc9e26fd934b8a7fad/src/Discord.Net.Commands/Builders/ModuleClassBuilder.cs
internal class PluginBuilder
{
    private readonly Module _module;
    private readonly List<CommandInfo> _commands;

    public PluginBuilder(Module module)
    {
        _module = module;
        _commands = new List<CommandInfo>();
    }

    public PluginInfo Build()
    {
        var pluginClass = _module.GetTypes().SingleOrDefault(IsValidPluginClass);

        if (pluginClass == null)
            throw new InvalidOperationException($"None, or more than one class inheriting {nameof(PluginBase)} were found in this module.");

        if (!pluginClass.TryGetCustomAttribute<PluginAttribute>(out var pluginAttribute))
            throw new InvalidOperationException($"Plugin class must be decorated with {nameof(PluginAttribute)}. Class: {pluginClass.FullName}");

        var summaryAttribute = pluginClass.GetCustomAttribute<SummaryAttribute>();

        foreach (var commandClass in _module.GetTypes().Where(IsValidCommandClass))
        {
            CommandGroupInfo? commandGroup = null;

            if (commandClass.TryGetCustomAttribute<CommandGroupAttribute>(out var commandGroupAttribute))
                commandGroup = BuildCommandGroup(commandClass, commandGroupAttribute);

            _commands.AddRange(BuildCommands(commandClass, commandGroup));
        }

        return new PluginInfo()
        {
            Name = pluginAttribute.Name,
            Version = pluginAttribute.Version,
            Author = pluginAttribute.Author,
            Summary = summaryAttribute?.Summary,
            Commands = _commands
        };
    }

    private CommandGroupInfo BuildCommandGroup(Type commandClass, CommandGroupAttribute commandGroupAttribute)
    {
        var commandGroupBuilder = new CommandGroupBuilder(commandGroupAttribute);

        foreach (var attribute in commandClass.GetCustomAttributes())
        {
            switch (attribute)
            {
                case SummaryAttribute summary:
                    commandGroupBuilder.WithSummary(summary);
                    break;
                case RequireRoleAttribute requireRole:
                    commandGroupBuilder.WithRequireRole(requireRole);
                    break;
            }
        }

        return commandGroupBuilder.Build();
    }

    private IEnumerable<CommandInfo> BuildCommands(Type commandClass, CommandGroupInfo? commandGroup = null)
    {
        foreach (var commandMethod in commandClass.GetMethods())
        {
            if (!commandMethod.TryGetCustomAttribute<CommandAttribute>(out var commandAttribute))
                continue;

            var commandBuilder = new CommandBuilder(commandAttribute);

            foreach (var attribute in commandMethod.GetCustomAttributes())
            {
                switch (attribute)
                {
                    case SummaryAttribute summary:
                        commandBuilder.WithSummary(summary);
                        break;
                    case RequireRoleAttribute requireRole:
                        commandBuilder.WithRequireRole(requireRole);
                        break;
                }
            }

            commandBuilder.WithParameters(BuildCommandParameters(commandMethod));

            var command = commandBuilder.Build();

            if (commandGroup != null)
            {
                commandBuilder.WithGroup(commandGroup);

                commandGroup.Commands ??= new List<CommandInfo>();
                commandGroup.Commands.Add(command);
            }

            yield return command;
        }
    }

    private IEnumerable<CommandParameterInfo> BuildCommandParameters(MethodInfo commandMethod)
    {
        foreach (var parameter in commandMethod.GetParameters())
        {
            var commandParameterBuilder = new CommandParameterBuilder(parameter);

            if (parameter.ParameterType == typeof(CommandContext))
                continue;

            foreach (var attribute in parameter.GetCustomAttributes())
            {
                switch (attribute)
                {
                    case SummaryAttribute summary:
                        commandParameterBuilder.WithSummary(summary);
                        break;
                    case RemainderAttribute remainder:
                        commandParameterBuilder.WithRemainder(remainder);
                        break;
                }
            }

            yield return commandParameterBuilder.Build();
        }
    }

    private bool IsValidPluginClass(Type type) =>
        type.IsClass &&
        type.IsPublic &&
        !type.IsAbstract &&
        !type.ContainsGenericParameters &&
        typeof(PluginBase).IsAssignableFrom(type);

    private bool IsValidCommandClass(Type type) =>
        type.IsClass &&
        type.IsPublic &&
        !type.IsAbstract &&
        !type.ContainsGenericParameters &&
        type.GetMethods().Any(m => m.GetCustomAttributes().Any(a => a is CommandAttribute));
}
