using Autowatchers.FileGenerators;
using Microsoft.CodeAnalysis;

namespace Autowatchers.Models;

internal class ClassSymbol
{
    public FileDataType Type { get; init; }

    public ClassData ClassData { get; init; }

    public string Namespace => ClassData.Namespace;

    public string ClassName => ClassData.ShortClassName;

    public string FullBuilderClassName => ClassData.FullClassName;

    public INamedTypeSymbol NamedTypeSymbol { get; init; } = null!;

    public string ItemBuilderFullName { get; init; } = string.Empty;

    public IReadOnlyList<IPropertySymbol> GetProperties(
        )
    {
        return NamedTypeSymbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.GetMethod is not null)
            .Where(x => x.GetMethod!.DeclaredAccessibility == Accessibility.Public)
            .Where(x => x.SetMethod is not null)
            .Where(x => x.SetMethod!.DeclaredAccessibility == Accessibility.Public)
            .Where(x => x.CanBeReferencedByName)
            .Where(x => !x.GetAttributes()
                .Any(a => AutowatcherAttributeGenerator.AutowatcherAttributeClassNames.Contains(a.AttributeClass?.OriginalDefinition.ToString())))
            .ToList();

    }
}
