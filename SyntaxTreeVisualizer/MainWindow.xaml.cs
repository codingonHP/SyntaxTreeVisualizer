using System;
using System.IO;
using System.Threading.Tasks;
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
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        string sourceCode = File.ReadAllText(openFileDialog.FileName);
                        RefreshCodeTree(sourceCode);
                        RefreshCodePreview(sourceCode);
                    });
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace,
                                    "Unable to parse the file",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error, MessageBoxResult.OK);
                }
        }

        private async void RefreshCodePreview(string sourceCode)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                SourceCodePreview.Text = _parser.FormatSourceCode(sourceCode);
            });

        }

        private async void RefreshCodeTree(string sourceCode)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                SourceCodeTree.Items.Clear();
                _parser.FillCodeTree(sourceCode, SourceCodeTree);
            });
        }

    }
}
