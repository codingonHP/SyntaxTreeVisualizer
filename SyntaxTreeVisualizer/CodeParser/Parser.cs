using System.Windows.Controls;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SyntaxTreeVisualizer.Colors;

namespace SyntaxTreeVisualizer.CodeParser
{
    public class Parser
    {
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

        private CSharpSyntaxTree CreateSyntaxTree(string sourceCode)
        {
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(sourceCode);
        }

        private void TraverseCodeDom(SyntaxNode currentRoot, TreeViewItem node)
        {
            var decendents = currentRoot.ChildNodesAndTokens();
            foreach (var syntaxNode in decendents)
            {
                var headerNode = new TreeViewItem
                {
                    Header = $"{syntaxNode.Kind()}",
                    Foreground = TreeNodeColor.ColorNode(syntaxNode)
                };

                node.Items.Add(headerNode);
                AddTriviaIfAvailable(syntaxNode, node);

                if (syntaxNode.IsNode)
                {
                    TraverseCodeDom((SyntaxNode)syntaxNode, node.Items[node.Items.Count - 1] as TreeViewItem);
                }
            }
        }

        private void AddTriviaIfAvailable(SyntaxNodeOrToken node, TreeViewItem treeNodeItem)
        {
            if (node.HasLeadingTrivia)
            {
                var trivia = node.GetLeadingTrivia()[0];
                WriteTriviaNode(treeNodeItem, trivia, "Leading Trivia - ");
            }

            if (node.HasTrailingTrivia)
            {
                var trivia = node.GetTrailingTrivia()[0];
                WriteTriviaNode(treeNodeItem, trivia, "Trailing Trivia - ");
            }
        }

        private void WriteTriviaNode(TreeViewItem treeNodeItem, SyntaxTrivia syntaxTrivia, string message = "")
        {
            var lastNode = treeNodeItem.Items[treeNodeItem.Items.Count - 1] as TreeViewItem;

            lastNode?.Items.Add(new TreeViewItem
            {
                Header = $"{message} { syntaxTrivia.Kind() } - {syntaxTrivia.FullSpan}",
                Foreground = TreeNodeColor.ColorNode(null)
            });
        }
    }
}
