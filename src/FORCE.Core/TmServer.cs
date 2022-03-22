using FORCE.Core.Utilities;
using GbxRemoteNet;

namespace FORCE.Core;

public class TmServer : GbxRemoteClient
{
    public PlayerList Players { get; private set; }

    private static readonly GbxRemoteClientOptions _defaultGbxRemoteClientOptions = new()
    {
        ConnectionRetries = 2,
        ConnectionRetryTimeout = TimeSpan.FromSeconds(1)
    };

    internal TmServer(string host, int port) : base(host, port, _defaultGbxRemoteClientOptions)
    {
        OnConnected += () =>
        {
            Players = new(this);

            return Task.CompletedTask;
        };
    }
}
