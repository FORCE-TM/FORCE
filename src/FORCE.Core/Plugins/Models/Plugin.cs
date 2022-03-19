namespace FORCE.Core.Plugins.Models;

internal class Plugin
{
    public string Name { get; }
    public string Version { get; }
    public string Author { get; }

    public Plugin(string name, string version, string author)
    {
        Name = name;
        Version = version;
        Author = author;
    }
}
