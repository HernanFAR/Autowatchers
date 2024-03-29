// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class AttributeArgumentListSyntaxExtensions
{
    public static string GetToWatchTypeName(this AttributeArgumentListSyntax? argumentList)
    {
        if (argumentList == null || argumentList.Arguments.Count is < 1 or > 2)
        {
            throw new ArgumentException("The WatchAttribute requires 1 or 2 arguments.");
        }

        if (argumentList.Arguments[0].Expression is TypeOfExpressionSyntax typeOfExpressionSyntax)
        {
            return typeOfExpressionSyntax.Type.ToString();
        }

        throw new ArgumentException("Invalid \"typeof\" syntax.");
    }

    public static bool IsDeepWatch(this AttributeArgumentListSyntax? argumentList)
    {
        if (argumentList == null || argumentList.Arguments.Count is < 1 or > 2)
        {
            throw new ArgumentException("The WatchAttribute requires 1 or 2 argument.");
        }

        if (argumentList.Arguments.Count == 1)
        {
            return false;
        }

        if (argumentList.Arguments[1].Expression is LiteralExpressionSyntax literalExpression)
        {
            return literalExpression.IsKind(SyntaxKind.TrueLiteralExpression);
        }

        return false;
    }

}