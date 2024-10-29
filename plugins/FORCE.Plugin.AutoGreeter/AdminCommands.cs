using FORCE.Core.Plugin;
using FORCE.Core.Shared;

namespace FORCE.Plugin.AutoGreeter;

[CommandGroup("autogreeter", "autogreet", "greeter", "ag")]
[RequireRole(PlayerRole.Admin)]
public class AdminCommands
{
    [Command("message", "msg")]
    [Summary("Set a new welcome message")]
    public async Task MessageAsync(CommandContext command,
        [Remainder] [Usage("new_message")] [Summary("The new auto welcome message (Use %player% for the player nickname)")]
        string newMessage)
    {
        // TODO
    }
}
