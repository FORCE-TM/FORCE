using FORCE.Core.Utilities;
using GbxRemoteNet;

namespace FORCE.Core;

public class TmServer : GbxRemoteClient
{
    public PlayerList Players { get; private set; }

    internal TmServer(string host, int port) : base(host, port)
    {
        WithOptions(new GbxRemoteClientOptions()
        {
            ConnectionRetries = 2,
            ConnectionRetryTimeout = TimeSpan.FromSeconds(1),
            CallbackInvoker = CustomCallbackInvoker
        });

        OnConnected += () =>
        {
            Players = new(this);

            return Task.CompletedTask;
        };
    }

    private async Task CustomCallbackInvoker(Delegate @delegate, params object[] args)
    {
        // Handle player connection/disconnection separately,
        // as we must ensure PlayerList is updated before any plugin receives the events

        if (@delegate is PlayerConnectAction)
            await Players.HandlePlayerConnectAsync((string)args[0]);
        else if (@delegate is PlayerDisconnectAction)
            await Players.HandlePlayerDisconnectAsync((string)args[0]);

        // No need to check for null, as all GbxRemoteClient events are null-safe by default:
        // https://github.com/FORCE-TM/GbxRemote.Net/commit/5e07a17243e91d016b75175cbefae10390d40cef
        @delegate.DynamicInvoke(args);
    }
}
