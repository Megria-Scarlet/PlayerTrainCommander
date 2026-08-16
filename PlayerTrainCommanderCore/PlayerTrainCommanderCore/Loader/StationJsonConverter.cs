using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    /// <summary>
    /// <see cref="Station"/> 型のオブジェクトと Json ファイルへの相互変換機能を提供します。
    /// </summary>
    public class StationJsonConverter : JsonConverter<Station>
    {
        public override Station? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(Station).IsAssignableFrom(typeToConvert))
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    string? id = null;
                    string? name = null;
                    string? shortname = null;
                    string? stationabbreviation = null;
                    uint capacity = 0;
                    uint demand = 0;


                    int startDepth = reader.CurrentDepth;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (reader.ValueTextEquals(nameof(id)))
                            {
                                reader.Read();
                                id = reader.GetString();
                            }
                            else if (reader.ValueTextEquals(nameof(name)))
                            {
                                reader.Read();
                                name = reader.GetString();
                            }
                            else if (reader.ValueTextEquals(nameof(shortname)))
                            {
                                reader.Read();
                                shortname = reader.GetString();
                            }
                            else if (reader.ValueTextEquals(nameof(stationabbreviation)))
                            {
                                reader.Read();
                                stationabbreviation = reader.GetString();
                            }
                            else if (reader.ValueTextEquals(nameof(capacity)))
                            {
                                reader.Read();
                                capacity = reader.GetUInt32();
                            }
                            else if (reader.ValueTextEquals(nameof(demand)))
                            {
                                reader.Read();
                                demand = reader.GetUInt32();
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
                    }

                    if (id is not null)
                        return new Station(id, name, shortname, capacity);
                    else
                        throw new FormatException("Station:id が取得できなかったお。");
                }
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Station value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
