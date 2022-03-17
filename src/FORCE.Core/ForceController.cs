using GbxRemoteNet;

namespace FORCE.Core;

public class ForceController
{
    public GbxRemoteClient Client { get; }

    public ForceController(string serverHost = "127.0.0.1", int serverPort = 5000)
    {
        Client = new GbxRemoteClient(serverHost, serverPort);
    }
}
