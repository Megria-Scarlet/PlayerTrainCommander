using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    public class ServiceTypeJsonConverter : JsonConverter<ServiceType>
    {
        /// <inheritdoc/>
        public override ServiceType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(ServiceType).IsAssignableFrom(typeToConvert))
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    string? id = null;
                    string? name = null;
                    string? shortname = null;
                    List<string> stations = new(8);


                    int startDepth = reader.CurrentDepth;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (reader.ValueTextEquals(nameof(id)))
                            {
                                reader.Read();
                                id = reader.GetString();
                                if (id is null)
                                    throw new JsonException("ID を null に設定することはできません。");
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
                            else if (reader.ValueTextEquals("stops"))
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.StartArray)
                                {
                                    JsonArrayStringEnumerator enumerator = new(reader);
                                    while (enumerator.MoveNext())
                                    {
                                        string? s = enumerator.Current;
                                        if (s is not null)
                                        {
                                            stations.Add(s);
                                        }
                                    }
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
                    }

                    if (id is not null)
                        return new ServiceType(id, name, shortname, stations);
                    else
                        throw new JsonException("プロパティ \"id\" が見つかりませんでした。");
                }
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, ServiceType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

}
