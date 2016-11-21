using System.Windows.Media;
using Microsoft.CodeAnalysis;

namespace SyntaxTreeVisualizer.Colors
{
    public static class TreeNodeColor
    {
        public static SolidColorBrush ColorNode(SyntaxNodeOrToken node)
        {
            if (node == null)
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            if (node.IsNode)
            {
                return new SolidColorBrush(Color.FromRgb(0, 0, 255));
            }

            if (node.IsToken)
            {
                return new SolidColorBrush(Color.FromRgb(29, 101, 7));
            }

            return new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

    }
}
