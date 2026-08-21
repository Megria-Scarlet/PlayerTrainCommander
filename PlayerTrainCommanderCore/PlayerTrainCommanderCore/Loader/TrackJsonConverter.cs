using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    /// <summary>
    /// <see cref="Track"/> 型のオブジェクトと Json ファイルへの相互変換機能を提供します。
    /// </summary>
    public class TrackJsonConverter : JsonConverter<Track>
    {
        /// <inheritdoc/>
        public override Track? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(Track).IsAssignableFrom(typeToConvert))
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    JsonElement element = JsonElement.ParseValue(ref reader);
                    return Read100(element, options);
                }
            }
            return null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Track value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        private Track Read100(in JsonElement element, JsonSerializerOptions options)
        {
            string? id = null;
            if (element.TryGetProperty("trackid", out JsonElement element1))
            {
                id = element1.GetString() ?? throw new NullReferenceException("ID を \"null\" に設定することはできません。");
            }
            else
            {
                throw new System.Collections.Generic.KeyNotFoundException("\"trackid\" プロパティが存在しません。");
            }
            const string Normal = "Normal";
            string? trackType;
            if (element.TryGetProperty("tracktype", out element1))
            {
                trackType = element1.GetString() ?? Normal;
            }
            else
            {
                trackType = Normal;
            }
            uint length;
            if (element.TryGetProperty("length", out element1))
            {
                length = element1.GetUInt32();
            }
            else
            {
                length = 0;
            }
            uint speedLimit;
            if (element.TryGetProperty("speedlimit", out element1))
            {
                speedLimit = element1.GetUInt32();
            }
            else
            {
                speedLimit = 0;
            }
            Track.Linker linker;
            if (element.TryGetProperty("joins", out element1))
            {
                linker = JsonSerializer.Deserialize<Track.Linker>(element1, options);
            }
            else
            {
                linker = default;
            }
            return ConvertTrack(in id, in trackType, length, speedLimit, in linker, in element, options);
        }
        /// <summary>
        /// <paramref name="trackType"/> に応じた <see cref="Track"/> 型を継承するオブジェクトを取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trackType"></param>
        /// <param name="length"></param>
        /// <param name="speedLimit"></param>
        /// <param name="linker"></param>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <returns><see cref="Track"/> 型を継承するオブジェクト。</returns>
        protected virtual Track ConvertTrack(in string id, in string trackType, uint length, uint speedLimit, in Track.Linker linker, in JsonElement element, JsonSerializerOptions options)
        {
            if (string.Equals(trackType, "Switch"))
            {
                return new BranchingTrack(id, length, speedLimit, linker);
            }
            else
            {
                return new Track(id, trackType, length, speedLimit, linker);
            }
        }
    }
}
