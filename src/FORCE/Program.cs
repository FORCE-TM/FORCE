﻿using FORCE.Core;

namespace FORCE;

internal static class Program
{
    public static async Task Main()
    {
        var force = new ForceController();

        Console.WriteLine("Connecting...");

        if (!await force.Server.ConnectAsync())
        {
            Console.WriteLine("Could not establish connection to the server.");
            return;
        }

        Console.WriteLine("Authenticating...");

        try
        {
            await force.Server.AuthenticateAsync("SuperAdmin", "SuperAdmin");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not authenticate to the server: {ex.Message}");
            return;
        }

        Console.WriteLine("Successfully connected and authenticated!" + Environment.NewLine);

        force.LoadPlugins();

        Console.WriteLine($"Online players: {force.Server.Players.Count}");

        foreach (var player in force.Server.Players)
        {
            Console.WriteLine($"   - [{player.Login}] {player.NickName} (Spectator: {player.IsSpectator})");
        }

        await force.Server.EnableCallbacksAsync();

        Console.WriteLine(Environment.NewLine + "Chat:");

        force.Server.OnPlayerChat += (playerUid, _, text, _) =>
        {
            if (playerUid != 0) // 0 = server
            {
                var player = force.Server.Players[playerUid];

                Console.WriteLine($"   [{player.NickName}] {text}");
            }

            return Task.CompletedTask;
        };

        await Task.Delay(-1);
    }
}
