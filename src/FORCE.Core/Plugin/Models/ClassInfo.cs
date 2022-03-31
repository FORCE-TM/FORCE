using System.Reflection;

namespace FORCE.Core.Plugin.Models;

internal class ClassInfo
{
    public ClassInfo(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    private object? _instance;
    public bool Instanced => _instance != null;
    public object GetInstance() => _instance ??= Activator.CreateInstance(Type)!;
    public T GetInstance<T>() => ((T)GetInstance())!;

    private const BindingFlags pmBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public IEnumerable<PersistentMember> GetPersistentMembers()
    {
        var instance = GetInstance();

        var members = instance.GetType().GetFields(pmBindingFlags).Cast<MemberInfo>()
            .Concat(instance.GetType().GetProperties(pmBindingFlags));

        foreach (var member in members.Where(m => m.GetCustomAttribute<PersistentAttribute>() != null))
        {
            yield return new PersistentMember(member, instance);
        }
    }

    public void SetPersistentMembers(IEnumerable<PersistentMember> persistentMembers)
    {
        var instance = GetInstance();

        foreach (var pm in persistentMembers)
        {
            if (pm.MemberType == MemberTypes.Field)
                instance.GetType().GetField(pm.Name, pmBindingFlags)?.SetValue(instance, pm.Value);
            else if (pm.MemberType == MemberTypes.Property)
                instance.GetType().GetProperty(pm.Name, pmBindingFlags)?.SetValue(instance, pm.Value);
        }
    }
}
