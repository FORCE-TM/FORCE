namespace FORCE.Core.Plugins.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PluginAttribute : Attribute
{
    public string Name { get; }
    public string Version { get; }
    public string Author { get; }

    public PluginAttribute(string name, string version, string author = null)
    {
        Name = name;
        Version = version;
        Author = author;
    }
}
