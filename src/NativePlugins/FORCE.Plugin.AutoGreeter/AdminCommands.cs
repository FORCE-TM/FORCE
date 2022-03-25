using FORCE.Core.Enums;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[CommandGroup("autogreeter", "autogreet", "greeter", "ag")]
[RequireRole(PlayerRole.Admin)]
public class AdminCommands : CommandContext
{
    [Command("message", "msg")]
    [Summary("Set a new welcome message")]
    public async Task MessageAsync(
        [Remainder]
        [Summary("The new auto welcome message (Use %player% for the player nickname)")]
        string newMessage)
    {
        // TODO
    }
}
