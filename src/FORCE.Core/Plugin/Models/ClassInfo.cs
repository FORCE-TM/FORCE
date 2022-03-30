namespace FORCE.Core.Plugin.Models;

internal class ClassInfo
{
    public ClassInfo(Type type)
    {
        Type = type;
        Commands = new List<CommandInfo>();
    }

    public Type Type { get; }
    public List<CommandInfo> Commands { get; }

    private object? _instance;
    public object Instance => _instance ??= Activator.CreateInstance(Type)!;
}
