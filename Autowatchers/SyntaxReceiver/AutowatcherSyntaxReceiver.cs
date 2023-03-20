using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Autowatchers.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Autowatchers.SyntaxReceiver;

internal class AutowatcherSyntaxReceiver : ISyntaxReceiver
{
    private static readonly string[] AutoGenerateBuilderAttributes = { "FluentBuilder.AutoGenerateBuilder", "AutoGenerateBuilder" };

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
            .FirstOrDefault(x => x.Attributes.Any(a => AutoGenerateBuilderAttributes.Contains(a.Name.ToString())));
        if (attributeList is null)
        {
            // ClassDeclarationSyntax should have the correct attribute
            return false;
        }

        var usings = new List<string>();

        string ns = classDeclarationSyntax.GetNamespace();

        if (!string.IsNullOrEmpty(ns))
        {
            usings.Add(ns);
        }

        if (classDeclarationSyntax.TryGetParentSyntax(out CompilationUnitSyntax? cc))
        {
            usings.AddRange(cc.Usings.Select(@using => @using.Name.ToString()));
        }

        // https://github.com/StefH/FluentBuilder/issues/36
        usings.AddRange(classDeclarationSyntax.GetAncestorsUsings().Select(@using => @using.Name.ToString()));

        usings = usings.Distinct().ToList();

        var rawTypeName  = AttributeArgumentListParser.ParseAttributeArguments(attributeList.Attributes.FirstOrDefault()?.ArgumentList);
        
        var modifiers = classDeclarationSyntax.Modifiers.Select(m => m.ToString()).ToArray();
        if (!(modifiers.Contains("public") && modifiers.Contains("partial")))
        {
            // ClassDeclarationSyntax should be "public" & "partial"
            return false;
        }

        data = new ClassData
        {
            Namespace = ns,
            ShortClassName = $"{classDeclarationSyntax.Identifier}",
            FullClassName = CreateFullBuilderClassName(ns, classDeclarationSyntax),
            MetadataName = ConvertTypeName(rawTypeName),
            Usings = usings,
        };

        return true;
    }

    private static string GetFullType(string ns, ClassDeclarationSyntax classDeclarationSyntax, bool addBuilderPostfix)
    {
        var fullBuilderClassName = CreateFullBuilderClassName(ns, classDeclarationSyntax);
        var type = $"{fullBuilderClassName}{(addBuilderPostfix ? "Builder" : string.Empty)}";

        if (classDeclarationSyntax.TypeParameterList != null)
        {
            var list = classDeclarationSyntax.TypeParameterList.Parameters.Select(p => p.Identifier.ToString());
            return $"{type}<{string.Join(",", list)}>";
        }

        return type;
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
