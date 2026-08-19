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
                    string? id = null;
                    string? trackType = null;
                    uint length = 0;
                    uint speedLimit = 0;
                    Track.Linker linker = default;


                    int startDepth = reader.CurrentDepth;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (reader.ValueTextEquals("trackid"))
                            {
                                reader.Read();
                                id = reader.GetString();
                                if (id is null)
                                    throw new JsonException("ID を null に設定することはできません。");
                            }
                            else if (reader.ValueTextEquals("tracktype"))
                            {
                                reader.Read();
                                trackType = reader.GetString();
                            }
                            else if (reader.ValueTextEquals("length"))
                            {
                                reader.Read();
                                length = reader.GetUInt32();
                            }
                            else if (reader.ValueTextEquals("speedlimit"))
                            {
                                reader.Read();
                                speedLimit = reader.GetUInt32();
                            }
                            else if (reader.ValueTextEquals("joins"))
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.StartObject)
                                {
                                    linker = JsonSerializer.Deserialize<Track.Linker>(ref reader, options);
                                    reader.Read();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                            }
                            else
                            {
                                reader.Skip();
                            }
                        }
                        else if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            if (reader.CurrentDepth <= startDepth)
                                break;
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                    if (id is not null)
                        return new Track(id, trackType ?? "Normal", length, speedLimit, linker);
                    else
                        throw new JsonException("プロパティ \"id\" が見つかりませんでした。");
                }
            }
            return null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Track value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
