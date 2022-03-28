using System.Diagnostics.CodeAnalysis;
using GbxRemoteNet;

namespace FORCE.Core.Plugin;

public class CommandContext : ContextBase
{
    /// <summary>
    /// The player who executed the command.
    /// </summary>
    [MaybeNull]
    public PlayerDetailedInfo Author { get; set; }

    /// <summary>
    /// Send a private message to the <see cref="Author"/>.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="arrowPrefix">Whether to prefix the message with a single arrow "> ". Used as a distinction between public and private messages.</param>
    public async Task ReplyAsync(string message, bool arrowPrefix = true)
        => await Server!.ChatSendServerMessageToIdAsync((arrowPrefix ? "$G> " : null) + message, Author!.PlayerId);

    /// <summary>
    /// Send a public message that everyone will see.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="arrowPrefix">Whether to prefix the message with a double arrow ">> ". Used as a distinction between public and private messages.</param>
    public async Task SendAsync(string message, bool arrowPrefix = true)
        => await Server!.ChatSendServerMessageAsync((arrowPrefix ? "$G>> " : null) + message);

    /// <summary>
    /// Send a message prefixed with the <see cref="Author"/> nickname, just like if he would talk in chat.
    /// </summary>
    /// <param name="message">Message to send.</param>
    public async Task SendAsAuthorAsync(string message)
        => await SendAsync($"$G[{Author!.NickName}$Z$S] {message}", false);
}
