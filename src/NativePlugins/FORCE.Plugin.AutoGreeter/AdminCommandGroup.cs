using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[CommandGroup("autogreeter", true), Alias("autogreet", "greeter")]
public class AdminCommandGroup : ForceCommand
{
    [Command("message"), Alias("msg")]
    [Summary("Set a new welcome message")]
    public async Task MessageAsync(
        [Remainder]
        [Summary("The new welcome message (Use %player% for the player nickname)")]
        string newMessage)
    {
        // TODO
    }
}
