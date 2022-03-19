using FORCE.Core;
using FORCE.Core.Plugins;
using FORCE.Core.Plugins.Attributes;

namespace FORCE.Plugin.AutoGreeter;

[Plugin("AutoGreeter", "1.0.0", "Laiteux")]
public class AutoGreeter : IPlugin
{
    public async Task OnPluginLoadAsync(ForceController force)
    {
        force.Server.OnPlayerConnect += async (login, spectator) =>
        {
            var player = await force.Server.GetPlayerInfoAsync(login);

            await force.Server.ChatSendServerMessageAsync($"$G>> Hello $FFF{player.NickName}$Z$G$S! Enjoy your stay (:");
        };
    }

    public Task OnPluginUnloadAsync(ForceController force)
    {
        throw new NotImplementedException();
    }
}
