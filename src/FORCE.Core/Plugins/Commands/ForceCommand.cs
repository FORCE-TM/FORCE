using FORCE.Core.Plugins.Commands.Models;

namespace FORCE.Core.Plugins.Commands;

public class ForceCommand
{
    protected CommandContext CommandContext { get; private set; }

    internal void SetCommandContext(CommandContext commandContext)
    {
        CommandContext = commandContext;
    }
}
