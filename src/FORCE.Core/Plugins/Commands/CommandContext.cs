using FORCE.Core.Plugins.Commands.Models;
using GbxRemoteNet;

namespace FORCE.Core.Plugins.Commands;

public class CommandContext : PluginContext
{
    internal CommandInfo Command { get; set; }

    public PlayerDetailedInfo Author { get; internal set; }

    public async Task ReplyWithUsageAsync()
        => await ReplyToAuthorAsync($"$F00Usage: $FFF{Command}");

    public async Task ReplyToAuthorAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageToIdAsync((arrowPrefix ? "$G> " : null) + message, Author.PlayerId);

    public async Task SendAsAuthorAsync(string message)
        => await SendToEveryoneAsync($"$G[{Author.NickName}$Z$S] {message}", false);

    public async Task SendToEveryoneAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageAsync((arrowPrefix ? "$G>> " : null) + message);
}
