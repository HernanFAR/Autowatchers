using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Reflection;

internal static class PropertyInfoExtensions
{
    public static bool IsInitOnly(this MethodInfo? methodInfo)
    {
        if (methodInfo == null)
            return false;

        var customModifiers = methodInfo.ReturnParameter
            .GetRequiredCustomModifiers();

        return customModifiers.Contains(typeof(IsExternalInit));
    }
}
