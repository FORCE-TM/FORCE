﻿using FORCE.Core.Plugins;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[Plugin("AutoGreeter", "1.0.0", "Laiteux")]
[Summary(@"Automatically says ""Hey"" when a player connects")]
public partial class AutoGreeter : ForcePlugin
{
    public override async Task OnPluginLoadAsync()
    {
        Server.OnPlayerConnect += async (login, _) => await GreetCommandAsync(login);
    }

    public override async Task OnPluginUnloadAsync()
    {
        // testing purposes
        await Server.ChatSendServerMessageAsync("byebye");
    }

    [Command("greet"), Alias("hey", "hi", "hello")]
    [Summary(@"Say ""Hey"" to a player")]
    public async Task GreetCommandAsync(string login)
    {
        var player = await Server.GetPlayerInfoAsync(login);

        if (CommandContext == null) // Called from event
        {
            await Server.ChatSendServerMessageAsync($"$G>> Hey $FFF{player.NickName}$Z$G$S! Enjoy your stay (:");
        }
        else
        {
            await Server.ChatSendServerMessageAsync($"$G[{CommandContext.Author.NickName}$Z$G$S] Hey $FFF{player.NickName}$Z$G$S!");
        }
    }
}
