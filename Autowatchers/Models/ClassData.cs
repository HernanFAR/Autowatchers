namespace Autowatchers.Models;

internal struct ClassData
{
    public string Namespace { get; init; }

    public string ShortClassName { get; init; }

    public string FullClassName { get; init; }

    public string MetadataName { get; init; }

    public List<string> Usings { get; init; }

}