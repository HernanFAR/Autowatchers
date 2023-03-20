using System.Diagnostics.CodeAnalysis;
using Autowatchers.Types;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Autowatchers.SyntaxReceiver;

internal static class AttributeArgumentListParser
{
    public static string ParseAttributeArguments(AttributeArgumentListSyntax? argumentList)
    {
        var result = new FluentBuilderAttributeArguments();

        if (argumentList == null || argumentList.Arguments.Count != 1)
        {
            throw new ArgumentException("The WatchAttribute requires 1 argument.");
        }

        if (argumentList.Arguments[0].Expression is TypeOfExpressionSyntax typeOfExpressionSyntax)
        {
            return typeOfExpressionSyntax.Type.ToString();
        }

        throw new ArgumentException("Invalid \"typeof\" syntax.");
    }
    
}