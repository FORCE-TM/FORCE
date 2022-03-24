using BoomTown.FuzzySharp;
using FORCE.Core.Plugins.Commands.Models;
using FORCE.Core.Plugins.Models;

namespace FORCE.Core.Plugins.Commands;

internal class CommandHandler
{
    private readonly PluginManager _pluginManager;
    private readonly List<PluginInfo> _plugins;

    public CommandHandler(PluginManager pluginManager)
    {
        _pluginManager = pluginManager;

        _plugins = _pluginManager.PluginAssemblies.SelectMany(a => a.Plugins).ToList();

        _pluginManager.OnPluginLoaded += (plugin, _) =>
            _plugins.Add(plugin);

        _pluginManager.OnPluginUnloaded += (plugin, _) =>
            _plugins.Remove(plugin);

        _pluginManager.Force.Server.OnPlayerChat += HandlePlayerChatAsync;
    }

    private async Task HandlePlayerChatAsync(int playerUid, string playerLogin, string text, bool isCommand)
    {
        if (text.Length <= 1 || !text.StartsWith('/'))
            return;

        // Trim command prefix
        text = text[1..];

        if (!TryFindCommand(text, out CommandInfo command, out string commandName, out string suggestionMatch))
        {
            string errorMessage = $"$G> $F00There is no such command as $FFF/{commandName ?? text}";

            if (suggestionMatch != null)
                errorMessage += $"$F00, did you mean $FFF/{suggestionMatch}$F00?";

            await _pluginManager.Force.Server.ChatSendServerMessageToLoginAsync(errorMessage, playerLogin);

            return;
        }

        Console.WriteLine($"Command /{commandName}");
    }

    private bool TryFindCommand(string text, out CommandInfo command, out string commandName, out string suggestion)
    {
        command = null; commandName = null; suggestion = null;
        int matchRatio = 0, suggestionRatio = 0;

        string[] textSplit = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Prioritize groups in case a group would be named the same as a command
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
}
