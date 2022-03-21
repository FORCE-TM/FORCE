using GbxRemoteNet;

namespace FORCE.Core;

public class TmServer : GbxRemoteClient
{
    internal TmServer(string host, int port) : base(host, port)
    {
    }

    internal TmServer(string host, int port, GbxRemoteClientOptions options) : base(host, port, options)
    {
    }
}
