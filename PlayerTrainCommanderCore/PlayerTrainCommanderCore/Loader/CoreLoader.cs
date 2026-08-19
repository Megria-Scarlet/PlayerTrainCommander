using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
    public class CoreLoader
    {
        private DirectoryInfo rootDirectory;

        private StationFile? stationFile;
        private ServiceTypeFile? serviceTypeFile;
        private Train[] trains;

        public CoreLoader(DirectoryInfo rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public void Load(ILoaderCallback callback)
        {
            try
            {
                List<DirectoryInfo> directorys = rootDirectory.EnumerateDirectories().ToList();
                this.stationFile = LoadStation(directorys);
                if (this.stationFile is not null)
                {
                    this.serviceTypeFile = LoadServiceType(directorys, this.stationFile.Stations);
                }
                this.trains = LoadTrain(directorys).ToArray();
            }
            catch (Exception ex)
            {
                callback.WriteError("An exception occurred.", ex);
            }
            StationFile? LoadStation(List<DirectoryInfo> directorys)
            {
                var staitonDirectory = directorys.FirstOrDefault(x => x.Name.Equals("station", StringComparison.InvariantCultureIgnoreCase));
                if (staitonDirectory is null)
                {
                    callback.WriteError("The \"staiton\" folder is missing.");
                    return null;
                }
                else
                {
                    directorys.Remove(staitonDirectory);
                    var stationFileInfo = staitonDirectory.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("station.json", StringComparison.InvariantCultureIgnoreCase));
                    if (stationFileInfo is null)
                    {
                        callback.WriteError("The \"station.json\" file is missing.");
                        return null;
                    }
                    else
                    {
                        using FileStream fileStream = stationFileInfo.OpenRead();
                        return StationFile.FromJson(fileStream);
                    }
                }
            }
            ServiceTypeFile? LoadServiceType(List<DirectoryInfo> directorys, scoped ReadOnlySpan<Station> stations)
            {
                var serviceTypeDirectory = directorys.FirstOrDefault(x => x.Name.Equals("ServiceType", StringComparison.InvariantCultureIgnoreCase));
                if (serviceTypeDirectory is null)
                {
                    callback.WriteError("The \"ServiceType\" folder is missing.");
                    return null;
                }
                else
                {
                    directorys.Remove(serviceTypeDirectory);
                    var serviceTypeFileInfo = serviceTypeDirectory.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("ServiceType.json", StringComparison.InvariantCultureIgnoreCase));
                    if (serviceTypeFileInfo is null)
                    {
                        callback.WriteError("The \"ServiceType.json\" file is missing.");
                        return null;
                    }
                    else
                    {
                        using FileStream fileStream = serviceTypeFileInfo.OpenRead();
                        return ServiceTypeFile.FromJson(fileStream, stations);
                    }
                }
            }
            IEnumerable<Train> LoadTrain(List<DirectoryInfo> directorys)
            {
                var trainDirectory = directorys.FirstOrDefault(x => x.Name.Equals("Train", StringComparison.InvariantCultureIgnoreCase));
                if (trainDirectory is null)
                {
                    callback.WriteError("The \"Train\" folder is missing.");
                    return [];
                }
                else
                {
                    directorys.Remove(trainDirectory);
                    return trainDirectory.EnumerateFiles("*.json").Select(Load).Where(x => x is not null)!;
                }
                static Train? Load(FileInfo fileInfo)
                {
                    using FileStream fileStream = fileInfo.OpenRead();
                    return System.Text.Json.JsonSerializer.Deserialize<Train>(fileStream);
                }
            }
        }
    }
}
