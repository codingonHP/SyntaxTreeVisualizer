using System;
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
        private readonly CodeParser.Parser _parser;
        public MainWindow()
        {
            InitializeComponent();
            _parser = new CodeParser.Parser();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".cs",
                Title = "Browse for C# files.",
                Filter = "C# code files|*.cs"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string sourceCode = File.ReadAllText(openFileDialog.FileName);
                    RefreshCodeTree(sourceCode);
                    RefreshCodePreview(sourceCode);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace, "Unable to parse the file", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        private void RefreshCodePreview(string sourceCode)
        {
            sourceCodePreview.Content = _parser.FormatSourceCode(sourceCode);
        }

        private void RefreshCodeTree(string sourceCode)
        {
            SourceCodeTree.Items.Clear();
            _parser.FillCodeTree(sourceCode, SourceCodeTree);
        }

    }
}
