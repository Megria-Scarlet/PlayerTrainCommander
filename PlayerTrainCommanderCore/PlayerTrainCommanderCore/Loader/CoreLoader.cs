using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
    public class CoreLoader
    {
        private DirectoryInfo rootDirectory;

        private StationFile? stationFile;
        private ServiceTypeFile? serviceTypeFile;
        private TrainData[]? trains;
        private TrackFile? trackFile;

        public CoreLoader(DirectoryInfo rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public void Load() => Load<ILoaderCallback>(null);
        public void Load<TCallback>(TCallback? callback) where TCallback : ILoaderCallback
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
                this.trackFile = LoadTrack(directorys);
            }
            catch (Exception ex)
            {
                callback?.WriteError("An exception occurred.", ex);
            }
            StationFile? LoadStation(List<DirectoryInfo> directorys)
            {
                var staitonDirectory = directorys.FirstOrDefault(x => x.Name.Equals("station", StringComparison.InvariantCultureIgnoreCase));
                if (staitonDirectory is null)
                {
                    callback?.WriteError("The \"staiton\" folder is missing.");
                    return null;
                }
                else
                {
                    directorys.Remove(staitonDirectory);
                    var stationFileInfo = staitonDirectory.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("station.json", StringComparison.InvariantCultureIgnoreCase));
                    if (stationFileInfo is null)
                    {
                        callback?.WriteError("The \"station.json\" file is missing.");
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
                    callback?.WriteError("The \"ServiceType\" folder is missing.");
                    return null;
                }
                else
                {
                    directorys.Remove(serviceTypeDirectory);
                    var serviceTypeFileInfo = serviceTypeDirectory.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("ServiceType.json", StringComparison.InvariantCultureIgnoreCase));
                    if (serviceTypeFileInfo is null)
                    {
                        callback?.WriteError("The \"ServiceType.json\" file is missing.");
                        return null;
                    }
                    else
                    {
                        using FileStream fileStream = serviceTypeFileInfo.OpenRead();
                        return ServiceTypeFile.FromJson(fileStream, stations);
                    }
                }
            }
            IEnumerable<TrainData> LoadTrain(List<DirectoryInfo> directorys)
            {
                var trainDirectory = directorys.FirstOrDefault(x => x.Name.Equals("Train", StringComparison.InvariantCultureIgnoreCase));
                if (trainDirectory is null)
                {
                    callback?.WriteError("The \"Train\" folder is missing.");
                    return [];
                }
                else
                {
                    directorys.Remove(trainDirectory);
                    return trainDirectory.EnumerateFiles("*.json").Select(Load).Where(x => x is not null)!;
                }
                static TrainData? Load(FileInfo fileInfo)
                {
                    using FileStream fileStream = fileInfo.OpenRead();
                    return System.Text.Json.JsonSerializer.Deserialize<TrainData>(fileStream);
                }
            }
            TrackFile? LoadTrack(List<DirectoryInfo> directorys)
            {
                var trackDirectory = directorys.FirstOrDefault(x => x.Name.Equals("track", StringComparison.InvariantCultureIgnoreCase));
                if (trackDirectory is null)
                {
                    callback?.WriteError("The \"track\" folder is missing.");
                    return null;
                }
                else
                {
                    directorys.Remove(trackDirectory);
                    var trackFileInfo = trackDirectory.EnumerateFiles().FirstOrDefault(x => x.Name.Equals("track.json", StringComparison.InvariantCultureIgnoreCase));
                    if (trackFileInfo is null)
                    {
                        callback?.WriteError("The \"track.json\" file is missing.");
                        return null;
                    }
                    else
                    {
                        using FileStream fileStream = trackFileInfo.OpenRead();
                        return TrackFile.FromJson(fileStream);
                    }
                }
            }
        }

        public ReadOnlySpan<Station> Stations
        {
            get => stationFile is null ? [] : stationFile.Stations;
        }
        public ReadOnlySpan<ServiceType> ServiceTypes
        {
            get => serviceTypeFile is null ? [] : serviceTypeFile.ServiceTypes;
        }
        public ReadOnlySpan<TrainData> Trains
        {
            get => new(trains);
        }
        public ReadOnlySpan<Track> Tracks
        {
            get => trackFile is null ? [] : trackFile.Tracks;
        }
    }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
}
