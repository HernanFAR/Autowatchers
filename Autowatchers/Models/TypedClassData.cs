namespace Autowatchers.Models;

internal record TypedClassData
{
    public bool IsSealed { get; init; }
    public IReadOnlyList<PropertyData> Properties { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Namespace { get; init; } = default!;
    public string FullTypeName => Namespace + "." + Name;

}
