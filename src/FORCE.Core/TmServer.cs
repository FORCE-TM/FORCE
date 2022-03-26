using FORCE.Core.Utilities;
using GbxRemoteNet;

namespace FORCE.Core;

public class TmServer : GbxRemoteClient
{
    public PlayerList Players { get; private set; }

    private readonly ObjectSemaphoreSlim _objectSemaphore;

    internal TmServer(string host, int port, ObjectSemaphoreSlim objectSemaphore) : base(host, port)
    {
        _objectSemaphore = objectSemaphore;

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

    private Delegate last = null;

    private async Task CustomCallbackInvoker(Delegate @delegate, params object[] args)
    {
        // Handle player connection/disconnection separately,
        // as we must ensure PlayerList is updated before any plugin receives the events
        if (@delegate is PlayerConnectAction)
            await Players.HandlePlayerConnectAsync((string)args[0]);
        else if (@delegate is PlayerDisconnectAction)
            await Players.HandlePlayerDisconnectAsync((string)args[0]);

        foreach (var invocation in @delegate.GetInvocationList())
        {
            _ = Task.Run(async () =>
            {
                // TODO: Check if class is thread-safe
                const bool threadSafe = true;

                if (threadSafe)
                    await _objectSemaphore.WaitAsync(invocation);

                await (Task)invocation.DynamicInvoke(args);

                if (threadSafe)
                    _objectSemaphore.Release(invocation);
            });
        }
    }
}
