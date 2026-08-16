using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
    [JsonConverter(typeof(StationFileJsonConverter))]
    public class StationFile
    {
        public string version;

        public Station[] stationlist;

        public StationFile(string version, Station[] stations)
        {
            this.version = version;
            this.stationlist = stations;
        }
    }

    public class StationFileJsonConverter : JsonConverter<StationFile>
    {
        public override StationFile? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(StationFile))
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    string? ver = null;
                    int startDepth = reader.CurrentDepth;
                    List<Station> stations = new List<Station>();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            if (startDepth <= reader.CurrentDepth)
                                break;
                        }
                        else if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (reader.ValueTextEquals("version"))
                            {
                                reader.Read();
                                ver = reader.GetString();
                            }
                            else if (reader.ValueTextEquals("stationlist"))
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.StartArray)
                                {
                                    reader.Read();
                                    do
                                    {
                                        Station? station = JsonSerializer.Deserialize<Station>(ref reader, options);
                                        if (station is not null)
                                            stations.Add(station);
                                        reader.Read();
                                    }
                                    while (reader.TokenType != JsonTokenType.EndArray);
                                }
                            }
                        }
                    }
                    if (ver is not null)
                        return new StationFile(ver, [.. stations]);

                    _ = ver;
                }
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, StationFile value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
}
