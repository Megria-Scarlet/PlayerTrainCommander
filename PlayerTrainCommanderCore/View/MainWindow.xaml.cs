using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog openFileDialog = new()
            {
                IsFolderPicker = true
            };
            if (openFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                string path = openFileDialog.FileName;
                System.IO.DirectoryInfo directoryInfo = new(path);
                if (directoryInfo.Exists)
                {
                    PTC.Core.Loader.CoreLoader loader = new(directoryInfo);
                    loader.Load(new MessageBoxCallback());
                    _ = loader;

                    foreach (string p in System.IO.Directory.EnumerateFiles(path, "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        Debug.WriteLine(p);
                    }
                }
            }
        }
        private struct MessageBoxCallback : PTC.Core.Loader.ILoaderCallback
        {
            public readonly void WriteError(string message)
            {
                MessageBox.Show(message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            public readonly void WriteError(string message, Exception exception)
            {
                MessageBox.Show(exception.Message, $"{exception.GetType()} エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            public readonly void WriteMessage(string message)
            {
                MessageBox.Show(message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            public readonly void WriteWarning(string message)
            {
                MessageBox.Show(message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}