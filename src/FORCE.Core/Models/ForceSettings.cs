using FORCE.Core.Shared;

namespace FORCE.Core.Models;

public class ForceSettings
{
    public ForceServerSettings Server { get; set; }
    public ColorScheme ColorScheme { get; set; }
}

public class ForceServerSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
}
