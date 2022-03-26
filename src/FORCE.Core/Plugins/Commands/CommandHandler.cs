using System.Reflection;
using BoomTown.FuzzySharp;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Models;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins.Commands;

internal class CommandHandler
{
    private ForceController Force => _pluginManager.Force;

    private readonly PluginManager _pluginManager;
    private readonly List<PluginInfo> _plugins;
    private readonly Dictionary<PluginInfo, List<PersistentMember>> _persistentMembers;

    public CommandHandler(PluginManager pluginManager)
    {
        _pluginManager = pluginManager;
        _persistentMembers = new();

        _plugins = _pluginManager.PluginAssemblies.SelectMany(a => a.Plugins).ToList();

        _pluginManager.OnPluginLoaded += (plugin, _) =>
            _plugins.Add(plugin);

        _pluginManager.OnPluginUnloaded += (plugin, reload) =>
        {
            _plugins.Remove(plugin);

            if (!reload)
            {
                _persistentMembers.Remove(plugin);
            }
            else if (_persistentMembers.ContainsKey(plugin))
            {
                _persistentMembers[plugin].RemoveAll(m => !m.BetweenReload);
            }
        };

        Force.Server.OnPlayerChat += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(int playerUid, string playerLogin, string text, bool _)
    {
        if (playerUid == 0) // server
            return;

        if (!text.StartsWith('/') || (text = text.TrimStart('/')).Length == 0)
            return;

        if (!TryMatchCommand(text, out CommandInfo command, out string commandName, out string suggestionMatch))
        {
            string errorMessage = $"$G> $F00There is no such command as $FFF/{commandName ?? text}";

            if (suggestionMatch != null)
                errorMessage += $"$F00, did you mean $FFF/{suggestionMatch}$F00?";

            await Force.Server.ChatSendServerMessageToIdAsync(errorMessage, playerUid);

            return;
        }

        if (!TryGetParameters(text, command, commandName, out var parameters))
        {
            string errorMessage = $"$G> $F00Usage: $FFF{command}";

            await Force.Server.ChatSendServerMessageToIdAsync(errorMessage, playerUid);

            return;
        }

        if (command.IsInGroup && command.Group.ThreadSafe)
            await Force.ObjectSemaphore.WaitAsync(command.Group);
        else if (command.ThreadSafe)
            await Force.ObjectSemaphore.WaitAsync(command);

        var pluginContext = CreatePluginContext(command, playerUid);
        LoadPersistentMembers(pluginContext);

        await (Task)command.Method.Invoke(pluginContext, parameters.ToArray());
        SavePersistentMembers(pluginContext);

        if (command.IsInGroup && command.Group.ThreadSafe)
            Force.ObjectSemaphore.Release(command.Group);
        else if (command.ThreadSafe)
            Force.ObjectSemaphore.Release(command);
    }

    private bool TryMatchCommand(string text, out CommandInfo command, out string commandName, out string suggestion)
    {
        command = null; commandName = null; suggestion = null;
        int matchRatio = 0, suggestionRatio = 0;

        string[] textSplit = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Prioritize groups in case a command would be named the same as a group and would match first
        foreach (var pluginCommand in _plugins.SelectMany(p => p.Commands).OrderByDescending(c => c.IsInGroup))
        {
            // TODO: Cache this, as it never changes
            string[] matchNames = pluginCommand.IsInGroup
                ? pluginCommand.Group.Prefixes.Select(prefix => string.Join(' ', prefix, pluginCommand.Name)).ToArray()
                : pluginCommand.Names;

            string userCommandName = textSplit.Length > 1 && pluginCommand.IsInGroup
                ? string.Join(' ', textSplit[0], textSplit[1])
                : textSplit[0];

            var (matchName, maxRatio) = matchNames.ToDictionary(m => m, m => Fuzzy.Ratio(m, userCommandName)).MaxBy(x => x.Value);

            if (maxRatio >= 75 && maxRatio > matchRatio)
            {
                (command, commandName, matchRatio) = (pluginCommand, userCommandName, maxRatio);
            }
            else if (maxRatio >= 50 && matchRatio == 0 && maxRatio > suggestionRatio)
            {
                (commandName, suggestion, suggestionRatio) = (userCommandName, matchName, maxRatio);
            }
        }

        return command != null;
    }

    private bool TryGetParameters(string text, CommandInfo command, string commandName, out List<object> parameters)
    {
        var userParameters = text
            .Split(' ')
            .Skip(commandName.Count(c => c == ' ') + 1)
            .Select(p => (object)p)
            .ToList();

        if (userParameters.Count < command.Parameters.Count(p => !p.HasDefaultValue))
        {
            parameters = null;
            return false;
        }

        if (userParameters.Count > command.Parameters.Count)
        {
            // Remove extra parameters
            userParameters = userParameters.SkipLast(userParameters.Count - command.Parameters.Count).ToList();
        }
        else if (command.Parameters.Count > userParameters.Count)
        {
            // Add default parameters
            command.Parameters.Skip(userParameters.Count).ForEach(p => userParameters.Add(p.DefaultValue));
        }

        parameters = userParameters;
        return true;
    }

    private PluginContext CreatePluginContext(CommandInfo command, int playerUid)
    {
        var pluginContext = (PluginContext)Activator.CreateInstance(command.Method.DeclaringType);

        pluginContext.Force = Force;
        pluginContext.Plugin = command.Plugin;

        var commandContext = new CommandContext()
        {
            Command = command,
            Author = Force.Server.Players[playerUid]
        };

        if (pluginContext is CommandContext cc)
        {
            // TODO: Avoid having to copy these values manually
            cc.Command = commandContext.Command;
            cc.Author = commandContext.Author;
        }
        else if (pluginContext is ForcePlugin fp)
        {
            fp.Command = commandContext;
            fp.Command.Force = pluginContext.Force;
            fp.Command.Plugin = pluginContext.Plugin;
        }

        return pluginContext;
    }

    private void LoadPersistentMembers(PluginContext pluginContext)
    {
        const BindingFlags bindingAttr = PersistentMember.BindingAttr;

        if (_persistentMembers.TryGetValue(pluginContext.Plugin, out var persistentMembers))
        {
            foreach (var member in persistentMembers.Where(m => m.ClassType == pluginContext.GetType()))
            {
                if (member.MemberType == MemberTypes.Property)
                    pluginContext.GetType().GetProperty(member.Name, bindingAttr)?.SetValue(pluginContext, member.Value);
                else if (member.MemberType == MemberTypes.Field)
                    pluginContext.GetType().GetField(member.Name, bindingAttr)?.SetValue(pluginContext, member.Value);
            }
        }
    }

    private void SavePersistentMembers(PluginContext pluginContext)
    {
        const BindingFlags bindingAttr = PersistentMember.BindingAttr;

        var members = pluginContext.GetType().GetProperties(bindingAttr).Cast<MemberInfo>()
            .Concat(pluginContext.GetType().GetFields(bindingAttr));

        if (!_persistentMembers.TryGetValue(pluginContext.Plugin, out var persistentMembers))
            persistentMembers = _persistentMembers[pluginContext.Plugin] = new();

        foreach (var member in members)
        {
            if (!member.TryGetCustomAttribute<PersistentAttribute>(out var persistentAttribute))
                continue;

            var persistentMember = new PersistentMember(member, pluginContext, persistentAttribute);

            persistentMembers.RemoveAll(m => m.Name == persistentMember.Name);
            persistentMembers.Add(persistentMember);
        }
    }
}
