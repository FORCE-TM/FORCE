using FORCE.Core.Plugin;

namespace FORCE.Plugin.AutoGreeter;

[Plugin("AutoGreeter", "1.0.0", "Laiteux")]
[Summary(@"Automatically says ""Hey"" when a player connects, and provides commands for players to greet others")]
public class Plugin : PluginBase
{
    [Persistent]
    private string? _lastLoggedInPlayer;

    public override async Task OnPluginLoadAsync(bool reload)
    {
        Server.OnPlayerConnect += async (login, _) =>
        {
            _lastLoggedInPlayer = login;

            await GreetAsync(null, login);
        };

        if (!reload)
            await Server.ChatSendServerMessageAsync($"$G>> Loaded {Plugin.ToColoredString(ColorScheme)} $G(:");
    }

    [Command("hey", "hi", "hello", "yo")]
    [Summary(@"Say ""Hey"" to the specified player (or the last who logged in)")]
    public async Task GreetAsync(CommandContext? command,
        [Summary("Login of the player to greet (if not specified, it will greet the last player who logged in)")]
        string? login = null)
    {
        var player = Server.Players![(login ?? _lastLoggedInPlayer)!];

        if (player == null)
        {
            if (command != null)
            {
                if (login == null)
                    await command.ReplyAsync("$F00No last logged in player found.");
                else
                    await command.ReplyAsync($"$F00Player $FFF{login} $F00not found.");
            }

            return;
        }

        // Means it was called from the event
        if (command == null)
        {
            await Server.ChatSendServerMessageAsync($"$G>> Hey $FFF{player.NickName}$Z$S! Enjoy your stay (:");
        }
        else if (player.Login == command.Author.Login)
        {
            if (login == null)
                await command.SendAsAuthorAsync("Hey everyone!");
            else
                await command.ReplyAsync("$F00You can not greet yourself.");
        }
        else
        {
            // That is so that "hey" will become "Hey"
            string greeting = char.ToUpper(command.Name[0]) + command.Name[1..].ToLower();

            await command.SendAsAuthorAsync($"{greeting} $FFF{player.NickName}$Z$S!");
        }
    }
}
