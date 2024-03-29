﻿using Autowatchers.FileGenerators;
using Autowatchers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics.CodeAnalysis;

namespace Autowatchers.Wrappers;

internal class GeneratorExecutionContextWrapper
{
    private readonly GeneratorExecutionContext _context;

    public GeneratorExecutionContextWrapper(GeneratorExecutionContext context)
    {
        _context = context;

        if (context.ParseOptions is not CSharpParseOptions csharpParseOptions)
        {
            throw new NotSupportedException("Only C# is supported.");
        }

        SupportsNullable = csharpParseOptions.LanguageVersion >= LanguageVersion.CSharp8;
    }

    public bool SupportsNullable { get; }

    public void AddSource(string hintName, SourceText sourceText) => _context.AddSource(hintName, sourceText);

    public bool TryGetNamedTypeSymbolByFullMetadataName(ClassData classData,
        [NotNullWhen(true)] out ClassSymbol? classSymbol)
    {
        classSymbol = null;

        // The GetTypeByMetadataName method returns null if no type matches the full name or if 2 or more types (in different assemblies) match the full name.
        var symbol = _context.Compilation.GetTypeByMetadataName(classData.MetadataName);

        if (symbol is not null)
        {
            classSymbol = new ClassSymbol
            {
                Type = FileDataType.Class,
                ClassData = classData,
                TypedClassData = new TypedClassData
                {
                    IsSealed = symbol.IsSealed,
                    Name = symbol.Name,
                    Namespace = symbol.GetFullNamespace(),
                    Properties = symbol
                        .GetMembers()
                        .OfType<IPropertySymbol>()
                        .Where(x => x.GetMethod is not null)
                        .Where(x => x.GetMethod!.DeclaredAccessibility == Accessibility.Public)
                        .Where(x => x.SetMethod is not null)
                        .Where(x => x.SetMethod!.DeclaredAccessibility == Accessibility.Public)
                        .Where(x => !x.SetMethod!.IsInitOnly)
                        .Where(x => x.CanBeReferencedByName)
                        .Where(p => !p.GetAttributes()
                            .Any(a =>
                                AutowatcherAttributeGenerator.AutowatcherExcludeAttributePropertyNames.Contains(a.AttributeClass?.OriginalDefinition.ToString())))
                        .Select(e => 
                            new PropertyData
                            {
                                TypeNamespace = e.GetTypeFullNamespace(),
                                Type = e.Type.MetadataName,
                                Name = e.Name,
                                IsVirtual = e.IsVirtual
                            })
                        .ToList()
                }
            };
            return true;
        }


        foreach (var @using in classData.Usings)
        {
            symbol = _context.Compilation
                .GetTypeByMetadataName($"{@using}.{classData.MetadataName}");

            if (symbol is null) continue;

            classSymbol = new ClassSymbol
            {
                Type = FileDataType.Class,
                ClassData = classData,
                TypedClassData = new TypedClassData
                {
                    IsSealed = symbol.IsSealed,
                    Name = symbol.Name,
                    Namespace = symbol.GetFullNamespace(),
                    Properties = symbol
                        .GetMembers()
                        .OfType<IPropertySymbol>()
                        .Where(x => x.GetMethod is not null)
                        .Where(x => x.GetMethod!.DeclaredAccessibility == Accessibility.Public)
                        .Where(x => x.SetMethod is not null)
                        .Where(x => x.SetMethod!.DeclaredAccessibility == Accessibility.Public)
                        .Where(x => !x.SetMethod!.IsInitOnly)
                        .Where(x => x.CanBeReferencedByName)
                        .Where(p => !p.GetAttributes()
                            .Any(a =>
                                AutowatcherAttributeGenerator.AutowatcherExcludeAttributePropertyNames.Contains(a.AttributeClass?.OriginalDefinition.ToString())))
                        .Select(e => new PropertyData
                        {
                            TypeNamespace = e.GetTypeFullNamespace(),
                            Type = e.Type.MetadataName,
                            Name = e.Name,
                            IsVirtual = e.IsVirtual
                        })
                        .ToList()
                }
            };

            return true;
        }

        return false;

    }

}
