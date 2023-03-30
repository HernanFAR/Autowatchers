using Autowatchers.Helpers;
using Autowatchers.Models;
using Autowatchers.SyntaxReceiver;
using Autowatchers.Wrappers;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Autowatchers.FileGenerators;

internal class AutowatcherClassesGenerator : IFilesGenerator
{
    private readonly GeneratorExecutionContextWrapper _context;
    private readonly AutowatcherSyntaxReceiver _receiver;

    public AutowatcherClassesGenerator(GeneratorExecutionContextWrapper context, AutowatcherSyntaxReceiver receiver)
    {
        _context = context;
        _receiver = receiver;
    }

    public IReadOnlyList<FileData> GenerateFiles()
    {
        var applicableClassSymbols = GetClassSymbols();

        return applicableClassSymbols.Select(classSymbol =>
            new FileData
            (
                FileDataType.Class,
                $"{classSymbol.ClassSymbol.FullName.Replace('<', '_').Replace('>', '_')}.g.cs",
                CreateClassBuilderCode(classSymbol.ClassSymbol)
            ))
            .ToArray();

    }

    private IEnumerable<(ClassSymbol ClassSymbol, ClassData ClassData)> GetClassSymbols()
    {
        foreach (var classItem in _receiver.CandidateClasses)
        {
            if (_context.TryGetNamedTypeSymbolByFullMetadataName(classItem, out var classSymbol))
            {
                yield return (classSymbol, classItem);
            }
        }
    }

    private string CreateClassBuilderCode(ClassSymbol classSymbol)
    {
        var classCode = new CodeBuilder(classSymbol, new PropertyCodeGenerator())
            .AppendReadOnlyPropertyCode()
            .AppendConstructorCode()
            .AppendPropertyCode();

        var deepWatchText = classSymbol.ClassData.ClassType == EClassType.Deep
            ? "<para>The deep watch can be expensive when used on large data structures. Use it only when necessary and beware of the performance implications.</para>"
            : "";
        
        return $@"{Header.Text}

{(_context.SupportsNullable ? "#nullable enable" : string.Empty)}

namespace {classSymbol.Namespace}
{{
    {(classSymbol.ClassData.OuterClassName is not null ? 
        $"public partial class {classSymbol.ClassData.OuterClassName}{{" : "")}
    /// <summary>
    /// Generated Autowatcher class for <see cref=""{classSymbol.TypedClassData.FullTypeName}"" />.
    /// </summary>
    /// <remarks>
    /// <para>If you want to extend the class, you can do it by adding methods and properties to 
    /// the attribute decorated {classSymbol.ClassName} partial class</para>
    /// {deepWatchText} 
    /// </remarks>
    public partial class {classSymbol.ClassName}
    {{
{classCode}
    }}
    {(classSymbol.ClassData.OuterClassName is not null ? "}" : "")}
}}
{(_context.SupportsNullable ? "#nullable disable" : string.Empty)}
        ";
    }
}
