using GbxRemoteNet;

namespace FORCE.Core.Plugins.Commands;

public class CommandContext : ForcePlugin
{
    public PlayerInfo Author { get; internal set; }

    public async Task ReplyAsync(string message)
        => await Server.ChatSendServerMessageToLoginAsync(message, Author.Login);
}
