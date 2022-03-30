namespace FORCE.Core.Plugin.Models;

internal class ClassInfo
{
    public ClassInfo(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    private object? _instance;
    public object Instance => _instance ??= Activator.CreateInstance(Type)!;
}
