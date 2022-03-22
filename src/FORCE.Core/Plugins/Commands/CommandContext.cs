using GbxRemoteNet;

namespace FORCE.Core.Plugins.Commands;

public class CommandContext
{
    public TmServer Server { get; private set; }

    public PlayerInfo Author { get; internal set; }

    public async Task ReplyAsync(string message)
        => await Server.ChatSendServerMessageToLoginAsync(message, Author.Login);

    internal void SetTmServer(TmServer server)
    {
        Server = server;
    }
}
