using FORCE.Core.Plugins;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[Plugin("AutoGreeter", "1.0.0", "Laiteux")]
[Summary(@"Automatically says ""Hey"" when a player connects, and provides commands for players to greet others")]
public class Plugin : ForcePlugin
{
    [Persistent(true)]
    private string _lastLoggedInPlayer;

    public override async Task OnPluginLoadAsync(bool reload)
    {
        Server.OnPlayerConnect += async (login, _) =>
        {
            _lastLoggedInPlayer = login;

            await GreetAsync(login);
        };

        if (!reload)
            await Server.ChatSendServerMessageAsync($"$G>> Loaded $FFF{Plugin} $G(:");
    }

    [Command("hey", "hi", "hello", "yo")]
    [Summary(@"Say ""Hey"" to the specified player (or the last who logged in)")]
    public async Task GreetAsync(
        [Summary("Login of the player to greet (if not specified, it will greet the last player who logged in)")]
        string login = null)
    {
        // TODO: Replace "Hey" by the command name, so that /yo would say "Yo"

        var player = Server.Players[login ?? _lastLoggedInPlayer];

        if (player == null)
        {
            if (Command != null)
            {
                if (login == null)
                    await Command?.ReplyToAuthorAsync("$F00No last logged in player found.");
                else
                    await Command?.ReplyToAuthorAsync($"$F00Player $FFF{login} $F00not found.");
            }

            return;
        }

        if (Command == null) // Means it was called from the event
        {
            await Server.ChatSendServerMessageAsync($"$G>> Hey $FFF{player.NickName}$Z$S! Enjoy your stay (:");
        }
        else if (player.Login == Command.Author.Login)
        {
            if (login == null)
                await Command.SendAsAuthorAsync("Hey everyone!");
            else
                await Command.ReplyToAuthorAsync("$F00You can not greet yourself.");
        }
        else
        {
            await Command.SendAsAuthorAsync($"Hey $FFF{player.NickName}$Z$S!");
        }
    }
}
