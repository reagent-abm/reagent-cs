using System.Reflection;

namespace TestReagent;

internal static class Utils
{
    internal static object? GetInstanceProperty(Type type, object instance, string propertyName) =>
        type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.GetValue(instance);
    
    internal static void SetInstanceProperty(Type type, object instance, string propertyName, object? value) =>
        type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(instance, value);
}