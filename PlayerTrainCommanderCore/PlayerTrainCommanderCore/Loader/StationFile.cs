using System;
using System.IO;
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
        private string version;

        private Station[] stationlist;

        public StationFile(string version, Station[] stations)
        {
            this.version = version;
            this.stationlist = stations;
        }
        /// <summary>
        /// <see cref="Station"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <returns>読み取られた <see cref="Station"/> 型のオブジェクト。</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<Station> GetStationList() => stationlist;

        /// <summary>
        /// json ファイルのストリームからデータを読み取り、 <see cref="StationFile"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <param name="utf8Json">json ファイルのストリーム。</param>
        /// <returns>json ファイルから読み取られた <see cref="StationFile"/> 型のオブジェクト。</returns>
        /// <param name="options">デシリアライズに使用するオプション。</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static StationFile? FromJson(Stream utf8Json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<StationFile>(utf8Json, options);
        }
    }

    public class StationFileJsonConverter : JsonConverter<StationFile>
    {
        public override StationFile? Read(scoped ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                                        else
                                            throw new FormatException("Station クラスを作成できなかったお。");
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
