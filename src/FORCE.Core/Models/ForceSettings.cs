using FORCE.Core.Shared;

namespace FORCE.Core.Models;

#pragma warning disable CS8618

public class ForceSettings
{
    public ForceServerSettings Server { get; set; }
    public ColorScheme ColorScheme { get; set; }

    public ForcePluginsSettings Plugins { get; set; }
}

public class ForceServerSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
}

public class ForcePluginsSettings
{
    public string Directory { get; set; }
    public string[] Enabled { get; set; }
}
