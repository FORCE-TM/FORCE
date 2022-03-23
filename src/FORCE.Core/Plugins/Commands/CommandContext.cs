using GbxRemoteNet;

namespace FORCE.Core.Plugins.Commands;

public class CommandContext : ForcePlugin
{
    public PlayerDetailedInfo Author { get; internal set; }

    public async Task ReplyPublicAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageAsync((arrowPrefix ? ">> " : null) + message);

    public async Task ReplyAuthorAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageToLoginAsync((arrowPrefix ? "> " : null) + message, Author.Login);
}
