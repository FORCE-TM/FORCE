using BoomTown.FuzzySharp;
using FORCE.Core.Extensions;
using FORCE.Core.Plugin.Models;

namespace FORCE.Core.Plugin;

internal class CommandHandler
{
    private readonly Force _force;
    private readonly List<CommandInfo> _commands;

    public CommandHandler(Force force)
    {
        _force = force;

        _commands = _force.PluginManager.GetLoadedPlugins().SelectMany(p => p.Commands).ToList();

        _force.PluginManager.OnPluginLoaded += plugin
            => _commands.AddRange(plugin.Commands);

        _force.PluginManager.OnPluginUnloaded += plugin
            => _commands.RemoveAll(c => c.Plugin == plugin);
    }

    public void StartListening()
        => _force.Server.OnPlayerChat += HandleCommandAsync;

    public void StopListening()
        => _force.Server.OnPlayerChat -= HandleCommandAsync;

    private async Task HandleCommandAsync(int playerUid, string playerLogin, string text, bool _)
    {
        if (playerUid == 0) // server
            return;

        if (!text.StartsWith('/') || (text = text.TrimStart('/')).Length == 0)
            return;

        if (!TryMatchCommand(text, out CommandInfo? command, out string? commandName, out string? suggestion))
        {
            string errorMessage = $"$G> $F00There is no such command as $FFF/{commandName ?? text}";

            if (suggestion != null)
                errorMessage += $"$F00, did you mean $FFF/{suggestion}$F00?";

            await _force.Server.ChatSendServerMessageToIdAsync(errorMessage, playerUid);

            return;
        }

        if (!TryGetParameters(text, command!, commandName!, out var parameters))
        {
            string errorMessage = $"$G> $F00Usage: $FFF{command}";

            await _force.Server.ChatSendServerMessageToIdAsync(errorMessage, playerUid);

            return;
        }

        var commandContext = new CommandContext()
        {
            Name = commandName!,
            Author = _force.Server.Players![playerUid]!,
            Plugin = command!.Plugin,
            Server = _force.Server,
            ColorScheme = _force.Settings.ColorScheme
        };

        await (Task)command.Method.Invoke(command.Class.GetInstance(), parameters!.Prepend(commandContext).ToArray())!;
    }

    private bool TryMatchCommand(string text, out CommandInfo? command, out string? commandName, out string? suggestion)
    {
        command = null; commandName = null; suggestion = null;
        int matchRatio = 0, suggestionRatio = 0;

        string[] textSplit = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Prioritize groups in case a command would be named the same as a group and would match first
        foreach (var pluginCommand in _commands.OrderByDescending(c => c.IsInGroup))
        {
            string[] matchNames = pluginCommand.IsInGroup
                ? pluginCommand.Group!.GroupPrefixes!
                    .SelectMany(prefix => pluginCommand.Names.Select(name => string.Join(' ', prefix, name)))
                    .ToArray()
                : pluginCommand.Names;

            string userCommandName = textSplit.Length > 1 && pluginCommand.IsInGroup
                ? string.Join(' ', textSplit[0], textSplit[1])
                : textSplit[0];

            var (matchName, maxRatio) = matchNames
                .ToDictionary(m => m, m => Fuzzy.Ratio(m, userCommandName))
                .MaxBy(x => x.Value);

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

    private bool TryGetParameters(string text, CommandInfo command, string commandName, out List<object>? parameters)
    {
        var userParameters = text
            .Split(' ')
            .Skip(commandName.Count(c => c == ' ') + 1)
            .Select(p => (object)p)
            .ToList();

        if (userParameters.Count < command.Parameters.Count(p => !p.HasDefaultValue))
        {
            parameters = null!;
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
            command.Parameters.Skip(userParameters.Count).ForEach(p => userParameters.Add(p.DefaultValue!));
        }

        parameters = userParameters;
        return true;
    }
}
