using System.Reflection;
using FORCE.Core.Plugins.Attributes;

namespace FORCE.Core.Plugins.Models;

internal class PersistentMember
{
    public const BindingFlags BindingAttr = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public Type ClassType { get; }

    public MemberTypes MemberType { get; }

    public string Name { get; }

    public object Value { get; }

    public bool BetweenReload { get; }

    public PersistentMember(MemberInfo member, object @class, PersistentAttribute persistentAttribute)
    {
        ClassType = @class.GetType();

        MemberType = member.MemberType;

        Name = member.Name;

        Value = member.MemberType switch
        {
            MemberTypes.Property => ((PropertyInfo)member).GetValue(@class),
            MemberTypes.Field => ((FieldInfo)member).GetValue(@class),
            _ => throw new NotSupportedException()
        };

        BetweenReload = persistentAttribute.BetweenReload;
    }
}
