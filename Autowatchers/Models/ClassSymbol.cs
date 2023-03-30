using Autowatchers.FileGenerators;
using Microsoft.CodeAnalysis;

namespace Autowatchers.Models;

internal class ClassSymbol
{
    public FileDataType Type { get; init; }

    public ClassData ClassData { get; init; }

    public string Namespace => ClassData.Namespace;

    public string ClassName => ClassData.ShortClassName;

    public string FullName => ClassData.FullClassName;

    public TypedClassData TypedClassData { get; init; } = default!;
    
}
