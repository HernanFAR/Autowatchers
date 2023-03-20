using Autowatchers.Models;
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{

    public static StringBuilder AppendConstructorCode(this StringBuilder builder,
        ClassSymbol classSymbol)
    {
        return builder;
    }

    public static StringBuilder AppendPropertyCode(this StringBuilder builder,
        ClassSymbol classSymbol, out List<string> extraUsings)
    {
        var properties = classSymbol.GetProperties();
        extraUsings = new List<string>();

        foreach (var property in properties)
        {
            builder.AppendLine("");
            builder.AppendLine();
        }

        return builder;
    }
}
