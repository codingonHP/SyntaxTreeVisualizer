using System.Windows.Controls;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SyntaxTreeVisualizer.CodeParser
{
    public class Parser
    {
        public CSharpSyntaxTree CreateSyntaxTree(string sourceCode)
        {
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(sourceCode);
        }

        public string FormatSourceCode(string sourceCode)
        {
            var syntaxTree = CreateSyntaxTree(sourceCode);
            return syntaxTree.ToString().Normalize();
        }

        public void FillCodeTree(string sourceCode, TreeView sourceCodeTree)
        {
            var syntaxTree = CreateSyntaxTree(sourceCode);
            var compilationUnit = syntaxTree.GetCompilationUnitRoot();

            sourceCodeTree.Items.Add(new TreeViewItem { Header = $"{compilationUnit.Kind()}" });

            TraverseCodeDom(compilationUnit, sourceCodeTree.Items[0] as TreeViewItem);
        }

        private void TraverseCodeDom(SyntaxNode currentRoot, TreeViewItem node)
        {
            var decendents = currentRoot.ChildNodesAndTokens();

            foreach (var syntaxNode in decendents)
            {
                var headerNode = new TreeViewItem { Header = $"{syntaxNode.Kind()}" };
                node.Items.Add(headerNode);

                if (syntaxNode.IsNode)
                {
                    TraverseCodeDom((SyntaxNode)syntaxNode, node.Items[node.Items.Count - 1] as TreeViewItem);
                }
            }
        }
    }
}
