using Autowatchers.FileGenerators;
using Autowatchers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Autowatchers.SyntaxReceiver;

internal class AutowatcherSyntaxReceiver : ISyntaxReceiver
{
    public IList<ClassData> CandidateClasses { get; } = new List<ClassData>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && TryGet(classDeclarationSyntax, out var data))
        {
            CandidateClasses.Add(data);
        }
    }

    private static bool TryGet(ClassDeclarationSyntax classDeclarationSyntax, out ClassData data)
    {
        data = default;

        var attributeList = classDeclarationSyntax.AttributeLists
            .FirstOrDefault(x => x.Attributes
                .Any(a =>
                    AutowatcherAttributeGenerator.AutowatcherAttributeClassNames.Contains(a.Name.ToString())));

        if (attributeList is null)
        {
            // ClassDeclarationSyntax should have the correct attribute
            return false;
        }

        var usings = new List<string>();

        var namespaceString = classDeclarationSyntax.GetNamespace();

        if (!string.IsNullOrEmpty(namespaceString))
        {
            usings.Add(namespaceString);
        }

        if (classDeclarationSyntax.TryGetParentSyntax(out CompilationUnitSyntax? cc))
        {
            usings.AddRange(cc.Usings.Select(@using => @using.Name.ToString()));
        }

        // https://github.com/StefH/FluentBuilder/issues/36
        usings.AddRange(classDeclarationSyntax.GetAncestorsUsings().Select(@using => @using.Name.ToString()));

        usings = usings.Distinct().ToList();

        var attributes = attributeList.Attributes.FirstOrDefault();

        if (attributes is null)
        {
            return false;
        }

        var rawTypeName = attributes.ArgumentList.GetToWatchTypeName();
        var modifiers = classDeclarationSyntax.Modifiers.Select(m => m.ToString()).ToArray();

        if (!(modifiers.Contains("public") && modifiers.Contains("partial")))
        {
            return false;
        }

        data = new ClassData
        {
            Namespace = namespaceString,
            ShortClassName = $"{classDeclarationSyntax.Identifier}",
            FullClassName = CreateFullBuilderClassName(namespaceString, classDeclarationSyntax),
            MetadataName = ConvertTypeName(rawTypeName),
            Usings = usings,
        };

        return true;
    }

    private static string CreateFullBuilderClassName(string ns, BaseTypeDeclarationSyntax classDeclarationSyntax)
    {
        return !string.IsNullOrEmpty(ns) ? $"{ns}.{classDeclarationSyntax.Identifier}" : classDeclarationSyntax.Identifier.ToString();
    }

    private static string ConvertTypeName(string typeName)
    {
        return !(typeName.Contains('<') && typeName.Contains('>')) ?
            typeName :
            $"{typeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(",", string.Empty).Trim()}`{typeName.Count(c => c == ',') + 1}";
    }
}
