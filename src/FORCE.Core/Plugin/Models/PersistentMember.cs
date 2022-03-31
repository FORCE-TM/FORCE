using System.Reflection;

namespace FORCE.Core.Plugin.Models;

public class PersistentMember
{
    public PersistentMember(MemberInfo member, object instance)
    {
        MemberType = member.MemberType;

        Name = member.Name;

        Value = MemberType switch
        {
            MemberTypes.Field => ((FieldInfo)member).GetValue(instance),
            MemberTypes.Property => ((PropertyInfo)member).GetValue(instance),
            _ => null! // Unreachable, considering the PersistentAttribute can only be used on Fields and Properties
        };
    }

    public MemberTypes MemberType { get; }
    public string Name { get; }
    public object? Value { get; }
}
