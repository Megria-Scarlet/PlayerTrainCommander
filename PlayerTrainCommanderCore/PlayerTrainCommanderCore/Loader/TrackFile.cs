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
    [JsonConverter(typeof(TrackFileJsonConverter))]
    internal class TrackFile
    {
        private string version;
        private Track[] tracks;
        public TrackFile(string version, Track[] tracks)
        {
            this.version = version;
            this.tracks = tracks;
        }
        /// <summary>
        /// <see cref="Track"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <returns>読み取られた <see cref="Track"/> 型のオブジェクト。</returns>
        public ReadOnlySpan<Track> Tracks
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => tracks;
        }
        /// <summary>
        /// バージョンを示す文字列を取得します。
        /// </summary>
        /// <returns>バージョンを示す文字列。</returns>
        public string Version
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => version;
        }
        /// <summary>
        /// json ファイルのストリームからデータを読み取り、 <see cref="TrackFile"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <param name="utf8Json">json ファイルのストリーム。</param>
        /// <returns>json ファイルから読み取られた <see cref="TrackFile"/> 型のオブジェクト。</returns>
        /// <param name="options">デシリアライズに使用するオプション。</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static TrackFile? FromJson(System.IO.Stream utf8Json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<TrackFile>(utf8Json, options);
        }
    }
    internal class TrackFileJsonConverter : JsonConverter<TrackFile>
    {
        public override TrackFile? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(TrackFile).IsAssignableFrom(typeToConvert))
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    string? ver = null;
                    int startDepth = reader.CurrentDepth;
                    List<Track> tracks = new List<Track>();

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
                            else if (reader.ValueTextEquals("data"))
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.StartArray)
                                {
                                    reader.Read();
                                    do
                                    {
                                        Track? track = JsonSerializer.Deserialize<Track>(ref reader, options);
                                        if (track is not null)
                                        {
                                            if (string.Equals(track.TrackType, "Switch", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                tracks.Add(new BranchingTrack(track.Id, track.Length, track.SpeedLimit, track.Link));
                                            }
                                            else
                                            {
                                                tracks.Add(track);
                                            }
                                        }
                                        reader.Read();
                                    }
                                    while (reader.TokenType != JsonTokenType.EndArray);
                                }
                            }
                        }
                    }
                    if (ver is not null)
                        return new TrackFile(ver, [.. tracks]);

                    _ = ver;
                }
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, TrackFile value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
}
