using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace SyntaxTreeVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CodeParser.Parser _roslyn;
        public MainWindow()
        {
            InitializeComponent();
            _roslyn = new CodeParser.Parser();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string sourceCode = File.ReadAllText(openFileDialog.FileName);
                RefreshCodeTree(sourceCode);
            }

        }

        private void RefreshCodeTree(string sourceCode)
        {
            SourceCodeTree.Items.Clear();
            _roslyn.ReachedAtNodeEvent += node =>
            {
                SourceCodeTree.Items.Add(node.Node);
            };

            _roslyn.FillCodeTree(sourceCode);
        }

    }
}
