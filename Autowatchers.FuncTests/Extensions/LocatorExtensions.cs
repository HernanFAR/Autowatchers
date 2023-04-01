// ReSharper disable once CheckNamespace
namespace Microsoft.Playwright;

public static class LocatorExtensions
{
    public static ILocatorAssertions Expect(this ILocator @this)
    {
        return Assertions.Expect(@this);
    } 
}
