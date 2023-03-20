using Autowatchers.Models;
using Autowatchers.SyntaxReceiver;
using Autowatchers.Wrappers;
using Microsoft.CodeAnalysis;
using System.Text;
using Autowatchers.Helpers;

namespace Autowatchers.FileGenerators;

internal class AutowatcherClassesGenerator : IFilesGenerator
{
    private readonly GeneratorExecutionContextWrapper _context;
    private readonly AutowatcherSyntaxReceiver _receiver;

    private static readonly string[] SystemUsings =
    {
        "System",
        "System.Collections.Generic"
    };

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
                FileDataType.Builder,
                $"{classSymbol.ClassSymbol.FullBuilderClassName.Replace('<', '_').Replace('>', '_')}.g.cs",
                CreateClassBuilderCode(classSymbol.ClassSymbol)
            ))
            .ToArray();

    }

    private IReadOnlyList<(ClassSymbol ClassSymbol, ClassData ClassData)> GetClassSymbols()
    {
        var classSymbols = new List<(ClassSymbol ClassSymbol, ClassData FluentData)>();

        foreach (var fluentDataItem in _receiver.CandidateClasses)
        {
            if (_context.TryGetNamedTypeSymbolByFullMetadataName(fluentDataItem, out var classSymbol))
            {
                classSymbols.Add((classSymbol, fluentDataItem));
            }
        }

        return classSymbols;
    }

    private string CreateClassBuilderCode(ClassSymbol classSymbol)
    {
        var classCode = new CodeBuilder(new StringBuilder())
            .AppendReadOnlyPropertyCode(classSymbol)
            .AppendConstructorCode(classSymbol)
            .AppendPropertyCode(classSymbol, out var extraUsings);

        var usings = SystemUsings.Concat(extraUsings)
            .ToList();

        var usingsAsStrings = string.Join("\r\n", usings.Distinct().Select(u => $"using {u};"));

        return $@"{Header.Text}

{(_context.SupportsNullable ? "#nullable enable" : string.Empty)}
{usingsAsStrings}

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
