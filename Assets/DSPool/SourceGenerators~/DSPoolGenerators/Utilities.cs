using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DSPoolGenerators;

public static class Utilities
{
    public static string GetNamespace(SyntaxNode syntaxNode)
    {
        SyntaxNode parent = syntaxNode.Parent;

        while (parent != null)
        {
            if (parent is NamespaceDeclarationSyntax namespaceDeclaration)
                return namespaceDeclaration.Name.ToString();

            parent = parent.Parent;
        }

        return null;
    }

}
