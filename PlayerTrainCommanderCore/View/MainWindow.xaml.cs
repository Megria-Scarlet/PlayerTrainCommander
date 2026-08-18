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
                    var stationFileInfo = staitonInfo.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("station.json", StringComparison.InvariantCultureIgnoreCase));
                    if (stationFileInfo is null)
                    {
                        string msg = "station.json ファイルがありません。";
                        MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    PTC.Core.Loader.StationFile stationFile;
                    {
                        using System.IO.FileStream fileStream = stationFileInfo.OpenRead();

                        JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
                        serializerOptions.Converters.Add(new PTC.Core.StationJsonConverter());
                        serializerOptions.Converters.Add(new PTC.Core.Loader.StationFileJsonConverter());
                        serializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);

                        stationFile = PTC.Core.Loader.StationFile.FromJson(fileStream, serializerOptions)!;
                    }

                    var serviceTypeInfo = directoryInfo.EnumerateDirectories().FirstOrDefault(x => x.Name.Equals("ServiceType", StringComparison.InvariantCultureIgnoreCase));
                    if (serviceTypeInfo is null)
                    {
                        string msg = "ServiceType フォルダーがありません。";
                        MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var serviceTypeFileInfo = serviceTypeInfo.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("ServiceType.json", StringComparison.InvariantCultureIgnoreCase));
                    if (serviceTypeFileInfo is null)
                    {
                        string msg = "ServiceType.json ファイルがありません。";
                        MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    PTC.Core.Loader.ServiceTypeFile serviceTypeFile;
                    {
                        using System.IO.FileStream fileStream = serviceTypeFileInfo.OpenRead();
                        serviceTypeFile = PTC.Core.Loader.ServiceTypeFile.FromJson(fileStream, stationFile.Stations)!;
                        _ = serviceTypeFile;
                    }

                    var trainInfo = directoryInfo.EnumerateDirectories().FirstOrDefault(x => x.Name.Equals("Train", StringComparison.InvariantCultureIgnoreCase));
                    if (trainInfo is null)
                    {
                        string msg = "Train フォルダーがありません。";
                        MessageBox.Show(msg, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    foreach (var trainTile in trainInfo.EnumerateFiles())
                    {
                        using System.IO.FileStream fileStream = trainTile.OpenRead();
                        var t = JsonSerializer.Deserialize<PTC.Core.Train>(fileStream);
                        _ = t;
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