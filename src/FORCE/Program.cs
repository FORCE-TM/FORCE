using FORCE.Core;
using GbxRemoteNet;

namespace FORCE;

internal static class Program
{
    public static async Task Main()
    {
        var force = new Force();

        if (!await ConnectAndAuthenticateAsync(force.Server))
            return;

        Console.WriteLine();
        await force.StartAsync();

        await Task.Delay(-1);
    }

    private static async Task<bool> ConnectAndAuthenticateAsync(GbxRemoteClient server)
    {
        Console.WriteLine("Connecting...");

        if (!await server.ConnectAsync())
        {
            Console.WriteLine("Could not establish connection to the server.");
            return false;
        }

        Console.WriteLine("Authenticating...");

        try
        {
            await server.AuthenticateAsync("SuperAdmin", "SuperAdmin");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not authenticate to the server: {ex.Message}");
            return false;
        }

        Console.WriteLine("Successfully connected and authenticated!");
        return true;
    }
}
