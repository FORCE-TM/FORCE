using FORCE.Core.Plugins;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[Plugin("AutoGreeter", "1.0.0", "Laiteux")]
[Summary(@"Automatically says ""Hey"" when a player connects")]
public class Plugin : ForcePlugin
{
    private string _lastLoggedInPlayer;

    public override async Task OnPluginLoadAsync(bool reload)
    {
#pragma warning disable CS1998

        Server.OnPlayerConnect += async (login, _)
            => _lastLoggedInPlayer = login;

        Server.OnPlayerConnect += async (login, _)
            => await GreetAsync(login);

        if (!reload)
            await Server.ChatSendServerMessageAsync($"$G>> Loaded $FFF{Plugin}");
    }

    [Command("greet", "hey", "hi", "hello", "yo")]
    [Summary(@"Say ""Hey"" to the specified player (or the last who logged in)")]
    public async Task GreetAsync(
        [Summary("Login of the player to greet (if not specified, it will greet the last player who logged in)")]
        string login = null)
    {
        var player = Server.Players[login ?? _lastLoggedInPlayer];

        if (player == null)
        {
            if (Command != null)
            {
                if (login == null)
                    await Command.ReplyAsync("$F00No last logged in player found.");
                else
                    await Command.ReplyAsync($"$F00Player $FFF{login} $F00not found.");
            }

            return;
        }

        if (Command == null) // Means it was called from the event
            await Server.ChatSendServerMessageAsync($"$G>> Hey $FFF{player.NickName}$Z$S! Enjoy your stay (:");
        else if (player.Login == Command.Author.Login)
            await Command.ReplyAsync("$F00You can not greet yourself.");
        else
            await Command.SendAsync($"$G[{Command.Author.NickName}$Z$S] Hey $FFF{player.NickName}$Z$S!", false);
    }
}
