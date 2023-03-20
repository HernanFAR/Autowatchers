using Autowatchers.Helpers;
using Autowatchers.Models;
using Autowatchers.SyntaxReceiver;
using Autowatchers.Wrappers;
using System.Text;

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
                $"{classSymbol.ClassSymbol.FullClassName.Replace('<', '_').Replace('>', '_')}.g.cs",
                CreateClassBuilderCode(classSymbol.ClassSymbol)
            ))
            .ToArray();

    }

    private IEnumerable<(ClassSymbol ClassSymbol, ClassData ClassData)> GetClassSymbols()
    {
        foreach (var fluentDataItem in _receiver.CandidateClasses)
        {
            if (_context.TryGetNamedTypeSymbolByFullMetadataName(fluentDataItem, out var classSymbol))
            {
                yield return (classSymbol, fluentDataItem);
            }
        }
    }

    private string CreateClassBuilderCode(ClassSymbol classSymbol)
    {
        var classCode = new CodeBuilder(new StringBuilder())
            .AppendReadOnlyPropertyCode(classSymbol)
            .AppendConstructorCode(classSymbol)
            .AppendPropertyCode(classSymbol);

        return $@"{Header.Text}

{(_context.SupportsNullable ? "#nullable enable" : string.Empty)}

namespace {classSymbol.Namespace}
{{
    public partial class {classSymbol.ClassName}
    {{
{classCode}
    }}
}}
{(_context.SupportsNullable ? "#nullable disable" : string.Empty)}
        ";
    }
}
