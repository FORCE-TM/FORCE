using GbxRemoteNet;

namespace FORCE.Core;

public class ForceController
{
    public GbxRemoteClient Server { get; }

    public ForceController(string serverHost = "127.0.0.1", int serverPort = 5000)
    {
        Server = new GbxRemoteClient(serverHost, serverPort);
    }
}
