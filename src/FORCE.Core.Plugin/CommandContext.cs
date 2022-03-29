using GbxRemoteNet;

namespace FORCE.Core.Plugin;

/// <summary>
/// Class containing info about the executed command, as well as some utility methods to interact with the server.<br/>
/// In order for a command method to be valid, its first parameter must always be of type <see cref="CommandContext"/>.
/// </summary>
public class CommandContext : ContextBase
{
    /// <summary>
    /// Name of the command that was typed by the <see cref="Author"/>.<br/>
    /// If command is part of a group, this will also contain the typed group prefix.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Player who executed the command.
    /// </summary>
    public PlayerDetailedInfo Author { get; set; } = null!;

    /// <summary>
    /// Send a private message to the <see cref="Author"/>.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="arrowPrefix">Whether to prefix the message with a single arrow "> ". Used as a distinction between public and private messages.</param>
    public async Task ReplyAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageToIdAsync((arrowPrefix ? "$G> " : null) + message, Author.PlayerId);

    /// <summary>
    /// Send a public message that everyone will see.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="arrowPrefix">Whether to prefix the message with a double arrow ">> ". Used as a distinction between public and private messages.</param>
    public async Task SendAsync(string message, bool arrowPrefix = true)
        => await Server.ChatSendServerMessageAsync((arrowPrefix ? "$G>> " : null) + message);

    /// <summary>
    /// Send a message prefixed with the <see cref="Author"/> nickname, just like if he would talk in chat.
    /// </summary>
    /// <param name="message">Message to send.</param>
    public async Task SendAsAuthorAsync(string message)
        => await SendAsync($"$G[{Author.NickName}$Z$S] {message}", false);
}
