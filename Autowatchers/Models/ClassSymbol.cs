using Autowatchers.FileGenerators;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

namespace Autowatchers.Models;

internal class ClassSymbol
{
    public FileDataType Type { get; init; }

    public ClassData ClassData { get; init; }

    public string Namespace => ClassData.Namespace;

    public string ClassName => ClassData.ShortClassName;

    public string FullBuilderClassName => ClassData.FullClassName;

    public INamedTypeSymbol NamedTypeSymbol { get; init; } = null!;

    public IReadOnlyList<PropertyData> Properties => GetProperties()
        .Select(e => new PropertyData
        {
            FullTypeName = e.Type.ContainingNamespace + "." + e.Type.Name,
            Name = e.Name
        })
        .ToList();

    private IEnumerable<IPropertySymbol> GetProperties()
    {
        return NamedTypeSymbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.GetMethod is not null)
            .Where(x => x.GetMethod!.DeclaredAccessibility == Accessibility.Public)
            .Where(x => x.SetMethod is not null)
            .Where(x => x.SetMethod!.DeclaredAccessibility == Accessibility.Public)
            .Where(x => x.CanBeReferencedByName)
            .Where(p => !p.GetAttributes()
                .Any(a => 
                    AutowatcherAttributeGenerator.AutowatcherExcludeAttributePropertyNames.Contains(a.AttributeClass?.OriginalDefinition.ToString())));
        
    }
}
