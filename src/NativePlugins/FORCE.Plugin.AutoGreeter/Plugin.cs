using FORCE.Core.Plugins;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[Plugin("AutoGreeter", "1.0.0", "Laiteux")]
[Summary(@"Automatically says ""Hey"" when a player connects")]
public class Plugin : ForcePlugin
{
    private string _lastLoggedInPlayer;

    public override Task OnPluginLoadAsync()
    {
        #pragma warning disable CS1998

        Server.OnPlayerConnect += async (login, _)
            => _lastLoggedInPlayer = login;

        Server.OnPlayerConnect += async (login, _)
            => await GreetCommandAsync(login);

        return Task.CompletedTask;
    }

    [Command("greet", "hey", "hi", "hello", "yo")]
    [Summary(@"Say ""Hey"" to the specified player (or the last who logged in)")]
    public async Task GreetCommandAsync(
        [Summary("Login of the player to greet (if not specified, it will greet the last player who logged in)")]
        string login = null)
    {
        var player = Server.Players[login ?? _lastLoggedInPlayer];

        if (player == null)
        {
            if (Command?.Author != null)
                await Command.ReplyAuthorAsync($"$F00Player $FFF{login} $F00not found.");

            return;
        }

        if (Command == null) // Means it was called from the event
            await Server.ChatSendServerMessageAsync($"$G>> Hey $FFF{player.NickName}$Z$G$S! Enjoy your stay (:");
        else
            await Server.ChatSendServerMessageAsync($"$G[{Command.Author.NickName}$Z$G$S] Hey $FFF{player.NickName}$Z$G$S!");
    }
}
