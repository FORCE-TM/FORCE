using System.Collections;
using GbxRemoteNet;
using GbxRemoteNet.XmlRpc;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using GbxRemoteNet.XmlRpc.Types;

namespace FORCE.Core.Shared;

public class PlayerList : IReadOnlyCollection<PlayerDetailedInfo>
{
    private readonly List<PlayerDetailedInfo> _players;
    private readonly GbxRemoteClient _tmServer;

    public PlayerList(GbxRemoteClient tmServer)
    {
        _players = new();

        _tmServer = tmServer;

        _tmServer.OnPlayerInfoChanged += newPlayerInfo =>
        {
            var player = _players[newPlayerInfo.PlayerId];

            player.TeamId = newPlayerInfo.TeamId;
            player.IsSpectator = newPlayerInfo.SpectatorStatus.Spectator;
            player.IsReferee = newPlayerInfo.Flags.IsReferee;

            return Task.CompletedTask;
        };

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

    internal async Task HandlePlayerConnectAsync(string login)
    {
        var player = await _tmServer.GetDetailedPlayerInfoAsync(login);

        _players.Add(player);
    }

    internal Task HandlePlayerDisconnectAsync(string login)
    {
        _players.RemoveAll(p => p.Login == login);

        return Task.CompletedTask;
    }

    public PlayerDetailedInfo? this[int uid]
        => _players.SingleOrDefault(p => p.PlayerId == uid);

    public PlayerDetailedInfo? this[string login]
        => _players.SingleOrDefault(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

    public IEnumerator<PlayerDetailedInfo> GetEnumerator() => _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _players.Count;
}
