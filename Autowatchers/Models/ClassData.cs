namespace Autowatchers.Models;

internal readonly struct ClassData
{
    public string Namespace { get; init; }

    public string ShortClassName { get; init; }

    public string FullClassName { get; init; }

    public string MetadataName { get; init; }

    public List<string> Usings { get; init; }

    public EClassType ClassType { get; init; }

    public string? OuterClassName { get; init; }

}