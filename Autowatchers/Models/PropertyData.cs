namespace Autowatchers.Models;

public readonly struct PropertyData
{
    public string FullTypeName => TypeNamespace + "." + Type;
    public string Name { get; init; }
    public string Type { get; init; }
    public bool IsVirtual { get; init; }
    public string TypeNamespace { get; init; }
}
