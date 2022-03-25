using FORCE.Core.Plugins.Commands.Models;
using GbxRemoteNet;

namespace FORCE.Core.Plugins.Commands;

public class CommandContext : PluginContext
{
    internal CommandInfo Command { get; set; }

    public PlayerDetailedInfo Author { get; internal set; }

    public async Task ReplyUsageAsync()
        => await ReplyAsync($"$F00Usage: $FFF{Command}");

    public async Task SendAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageAsync((arrowPrefix ? "$G>> " : null) + message);

    public async Task ReplyAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageToLoginAsync((arrowPrefix ? "$G> " : null) + message, Author.Login);
}
