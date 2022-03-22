using FORCE.Core.Plugins.Commands.Models;

namespace FORCE.Core.Plugins.Commands;

internal class CommandHandler
{
    private readonly PluginManager _pluginManager;

    public CommandHandler(PluginManager pluginManager)
    {
        _pluginManager = pluginManager;
    }

    public void StartListening()
    {
        _pluginManager.Force.Server.OnPlayerChat += HandlePlayerChatAsync;
    }

    private async Task HandlePlayerChatAsync(int playerUid, string playerLogin, string text, bool isCommand)
    {
        if (text.Length <= 1 || !text.StartsWith('/'))
            return;

        // TODO: Don't split if Remainder
        string[] cmdParams = text.Substring(1).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string cmdName = cmdParams[0].ToLower();
        cmdParams = cmdParams[1..];

        // TODO: Find closest command name using fuzzy matching
        if (!TryFindCommand(cmdName, cmdParams, out var command))
        {
            await _pluginManager.Force.Server.ChatSendServerMessageToLoginAsync(
                $"$G> $F00There is no such command as $FFF/{cmdName}"/* + $"$F00, did you mean $FFF/{closestCmdName}$F00?"*/,
                playerLogin);

            return;
        }

        Console.WriteLine($"Command {text} from player {playerLogin}");
    }

    // TODO: Support for command groups
    // TODO: If only 1 name match but not enough arguments, send help message
    // TODO: If more than 1 name match but not enough arguments, send help dialog?
    // TODO: Cache commands:
    // PluginManager.OnPluginLoaded
    // PluginManager.OnPluginUnloaded
    // PluginManager.OnPluginReloaded
    // "12 commands discovered (3 new)"
    private bool TryFindCommand(string cmdName, string[] cmdParams, out CommandInfo command)
    {
        var allCommands = _pluginManager.PluginAssemblies.SelectMany(a => a.Plugins).SelectMany(p => p.Commands);

        var nameMatchCommands = allCommands.Where(c => c.Names.Contains(cmdName, StringComparer.OrdinalIgnoreCase)).ToList();

        if (nameMatchCommands.Count == 0)
        {
            command = null;
            return false;
        }

        command = nameMatchCommands.SingleOrDefault(cmd =>
        {
            int cmdParamCount;

            for (cmdParamCount = 0; cmdParamCount < cmd.Parameters.Count; cmdParamCount++)
            {
                var parameter = cmd.Parameters[cmdParamCount];

                if (parameter.HasDefaultValue) break;
            }

            return cmdParams.Length >= cmdParamCount;
        });

        return command != null;
    }
}
