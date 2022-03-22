using System.Collections;
using GbxRemoteNet;
using GbxRemoteNet.XmlRpc;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using GbxRemoteNet.XmlRpc.Types;

namespace FORCE.Core.Utilities;

public class PlayerList : IReadOnlyCollection<PlayerDetailedInfo>
{
    private readonly List<PlayerDetailedInfo> _players;
    private readonly GbxRemoteClient _tmServer;

    public PlayerList(GbxRemoteClient tmServer)
    {
        _players = new();

        _tmServer = tmServer;
        _tmServer.OnPlayerConnect += HandlePlayerConnectAsync;
        _tmServer.OnPlayerDisconnect += HandlePlayerDisconnectAsync;

        LoadOnlinePlayersAsync().GetAwaiter().GetResult();
    }

    private async Task LoadOnlinePlayersAsync()
    {
        var players = await _tmServer.GetPlayerListAsync();

        var multicall = new MultiCall();

        foreach (var player in players)
        {
            multicall.Add(_tmServer.GetDetailedPlayerInfoAsync, player.Login);
        }

        object[] results = await _tmServer.MultiCallAsync(multicall);

        foreach (var result in results.Cast<DynamicObject>())
        {
            var player = (PlayerDetailedInfo)XmlRpcTypes.ToNativeStruct<PlayerDetailedInfo>(new XmlRpcStruct(result));

            _players.Add(player);
        }
    }

    private async Task HandlePlayerConnectAsync(string login, bool _)
    {
        var player = await _tmServer.GetDetailedPlayerInfoAsync(login);

        _players.Add(player);
    }

    private async Task HandlePlayerDisconnectAsync(string login)
    {
        _players.RemoveAll(p => p.Login == login);
    }

    public PlayerDetailedInfo this[int uid]
        => _players.SingleOrDefault(p => p.PlayerId == uid);

    public PlayerDetailedInfo this[string login]
        => _players.SingleOrDefault(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

    public IEnumerator<PlayerDetailedInfo> GetEnumerator() => _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _players.Count;
}
