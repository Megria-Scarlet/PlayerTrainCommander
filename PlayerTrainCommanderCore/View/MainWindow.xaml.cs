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
                    var list = directoryInfo.GetDirectories().ToList();
                    var staitonInfo = list.FirstOrDefault(x => x.Name.Equals("station", StringComparison.InvariantCultureIgnoreCase));
                    if (staitonInfo is null)
                    {
                        string msg = "staiton フォルダーがありません。";
                        MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var stationFile = staitonInfo.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("station.json", StringComparison.InvariantCultureIgnoreCase));
                    if (stationFile is null)
                    {
                        string msg = "staiton ファイルがありません。";
                        MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    {
                        using System.IO.FileStream fileStream = stationFile.OpenRead();
                        JsonSerializerOptions serializerOptions = new();
                        serializerOptions.Converters.Add(new PTC.Core.StationJsonConverter());
                        var obj = JsonSerializer.Deserialize<PTC.Core.Loader.StationFile>(fileStream, serializerOptions);
                        _ = obj;
                    }

                    var serviceTypeInfo = directoryInfo.EnumerateDirectories().FirstOrDefault(x => x.Name.Equals("ServiceType", StringComparison.InvariantCultureIgnoreCase));
                    if (serviceTypeInfo is not null)
                    {

                    }



                    foreach (string p in System.IO.Directory.EnumerateFiles(path, "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        Debug.WriteLine(p);
                    }
                }
            }
        }
    }
}