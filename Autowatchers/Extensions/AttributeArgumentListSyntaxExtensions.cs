// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class AttributeArgumentListSyntaxExtensions
{
    public static string GetToWatchTypeName(this AttributeArgumentListSyntax? argumentList)
    {
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