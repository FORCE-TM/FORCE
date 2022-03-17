using FORCE.Core;
using GbxRemoteNet.Structs;

namespace FORCE;

internal static class Program
{
    public static async Task Main()
    {
        var force = new ForceController();

        Console.WriteLine("Connecting...");

        if (!await force.Client.ConnectAsync())
        {
            Console.WriteLine("Could not establish a connection to the server.");
            return;
        }

        Console.WriteLine("Authenticating...");

        try
        {
            await force.Client.AuthenticateAsync("SuperAdmin", "SuperAdmin");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not authenticate to the server: {ex.Message}");
            return;
        }

        Console.WriteLine("Successfully connected and authenticated!" + Environment.NewLine);

        PlayerInfo[] playerList = await force.Client.GetPlayerListAsync();
        Console.WriteLine($"Online players: {playerList.Length}");

        foreach (var player in playerList)
        {
            Console.WriteLine($"   - [{player.Login}] {player.NickName} (Spectator: {player.SpectatorStatus.Spectator})");
        }

        await force.Client.EnableCallbacksAsync();
        
        Console.WriteLine(Environment.NewLine + "Chat:");

        force.Client.OnPlayerChat += async (playerUid, login, text, isRegisteredCmd) =>
        {
            var player = await force.Client.GetPlayerInfoAsync(login);

            Console.WriteLine($"   [{player.NickName}] {text}");
        };

        await Task.Delay(-1);
    }
}
