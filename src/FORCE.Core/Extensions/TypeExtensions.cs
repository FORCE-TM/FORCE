using System.Reflection;

namespace FORCE.Core.Extensions;

internal static class TypeExtensions
{
    public static bool TryGetCustomAttribute<T>(this Type type, out T attribute) where T : Attribute
    {
        return (attribute = type.GetCustomAttribute<T>()) != null;
    }
}
