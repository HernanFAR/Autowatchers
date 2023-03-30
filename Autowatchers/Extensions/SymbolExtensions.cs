// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis;

internal static class SymbolExtensions
{
    public static string GetFullNamespace(this ISymbol @this)
    {
        var namespaceSymbol = @this.ContainingNamespace;

        if (namespaceSymbol is null) throw new InvalidOperationException(nameof(namespaceSymbol));

        var namespaces = new List<string>()
        {
            namespaceSymbol.Name
        };

        while (true)
        {
            namespaceSymbol = namespaceSymbol.ContainingNamespace;

            if (namespaceSymbol is null || string.IsNullOrEmpty(namespaceSymbol.Name)) break;

            namespaces.Add(namespaceSymbol.Name);
        }

        namespaces.Reverse();

        var fullNameSpace = namespaces.First();

        return namespaces.Skip(1).Aggregate(fullNameSpace, (current, namespaceString) => current + ("." + namespaceString));
    }

    public static string GetTypeFullNamespace(this IPropertySymbol @this)
    {
        var namespaceSymbol = @this.Type
            .ContainingNamespace;

        if (namespaceSymbol is null) throw new InvalidOperationException(nameof(namespaceSymbol));

        var namespaces = new List<string>()
        {
            namespaceSymbol.Name
        };

        while (true)
        {
            namespaceSymbol = namespaceSymbol.ContainingNamespace;

            if (namespaceSymbol is null || string.IsNullOrEmpty(namespaceSymbol.Name)) break;

            namespaces.Add(namespaceSymbol.Name);
        }

        namespaces.Reverse();

        var fullNameSpace = namespaces.First();

        return namespaces.Skip(1).Aggregate(fullNameSpace, (current, namespaceString) => current + ("." + namespaceString));
    }
}
