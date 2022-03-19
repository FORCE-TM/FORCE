using GbxRemoteNet;

namespace FORCE.Core;

// Just a wrapper for GbxRemoteClient, so that the class name makes more sense
public class TmServer : GbxRemoteClient
{
    internal TmServer(string host, int port) : base(host, port)
    {
    }

    internal TmServer(string host, int port, GbxRemoteClientOptions options) : base(host, port, options)
    {
    }
}
