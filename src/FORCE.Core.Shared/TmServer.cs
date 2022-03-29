using GbxRemoteNet;

namespace FORCE.Core.Shared;

/// <summary>
/// A all-in-one class for calling server methods and subscribing to callback events.
/// </summary>
public sealed class TmServer : GbxRemoteClient
{
    /// <inheritdoc cref="PlayerList"/>
    public PlayerList? Players { get; private set; }

    public TmServer(string host, int port) : base(host, port)
    {
        this.WithOptions(new GbxRemoteClientOptions()
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
        // Handle the player connection/disconnection events first,
        // as we must ensure Players is updated before any plugin receives these events.
        if (@delegate is PlayerConnectAction)
            await Players!.HandlePlayerConnectAsync((string)args[0]);
        else if (@delegate is PlayerDisconnectAction)
            await Players!.HandlePlayerDisconnectAsync((string)args[0]);

        // Then we can invoke all the methods subscribed to the event.
        // No need to check for null, as all GbxRemoteClient events are null-safe by default:
        // https://github.com/FORCE-TM/GbxRemote.Net/commit/5e07a17243e91d016b75175cbefae10390d40cef
        @delegate.DynamicInvoke(args);
    }
}
