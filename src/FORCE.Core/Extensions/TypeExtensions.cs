using System.Reflection;

namespace FORCE.Core.Extensions;

internal static class TypeExtensions
{
    public static bool TryGetCustomAttribute<T>(this MemberInfo element, out T attribute) where T : Attribute
    {
        return (attribute = element.GetCustomAttribute<T>()!) != null;
    }

    public static bool TryGetCustomAttribute<T>(this ParameterInfo element, out T attribute) where T : Attribute
    {
        return (attribute = element.GetCustomAttribute<T>()!) != null;
    }
}
