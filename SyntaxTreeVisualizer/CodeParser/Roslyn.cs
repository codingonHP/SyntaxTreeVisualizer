using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SyntaxTreeVisualizer.CodeParser
{
    public class NodeInfoEventArgs : EventArgs
    {
        public string Node { get; set; }
    }

    public class Parser
    {
        public event Action<NodeInfoEventArgs> ReachedAtNodeEvent;
        private readonly NodeInfoEventArgs _nodeInfoEventArgs;

        public Parser()
        {
            _nodeInfoEventArgs = new NodeInfoEventArgs();
        }
        public CSharpSyntaxTree CreateSyntaxTree(string sourceCode)
        {
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(sourceCode);
        }

        public string FormatSourceCode(string sourceCode)
        {
            var syntaxTree = CreateSyntaxTree(sourceCode);
            return syntaxTree.ToString().Normalize();
        }

        public string FillCodeTree(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                return string.Empty;
            }

            var syntaxTree = CreateSyntaxTree(sourceCode);
            var compilationUnit = syntaxTree.GetCompilationUnitRoot();

            return TraverseCodeDom(compilationUnit);
        }

        private string TraverseCodeDom(SyntaxNode currentRoot)
        {
            var decendents = currentRoot.DescendantNodes();
            foreach (var syntaxNode in decendents)
            {
                var gotNode = TraverseCodeDom(syntaxNode);
                _nodeInfoEventArgs.Node = gotNode;
                ReachedAtNodeEvent?.Invoke(_nodeInfoEventArgs);
            }

            return $"{currentRoot.Kind()} - {currentRoot.ToString().Normalize()}";
        }
    }
}
