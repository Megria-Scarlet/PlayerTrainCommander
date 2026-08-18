using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    /// <summary>
    /// <see cref="ServiceType"/> 型のオブジェクトと Json ファイルへの相互変換機能を提供します。
    /// </summary>
    public class ServiceTypeJsonConverter : JsonConverter<ServiceType>
    {
        private Station[] stations;

        /// <summary>
        /// <see cref="Station"/> コレクションを指定して、新しい <see cref="ServiceTypeJsonConverter"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="stations"><see cref="Station"/> コレクション。</param>
        public ServiceTypeJsonConverter(IEnumerable<Station> stations)
        {
            this.stations = [.. stations];
        }
        /// <inheritdoc cref="ServiceTypeJsonConverter(IEnumerable{Station})"/>
        public ServiceTypeJsonConverter(scoped ReadOnlySpan<Station> stations)
        {
            this.stations = stations.ToArray();
        }


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
                    List<Station> stations = new(8);


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
                                    JsonStringArrayEnumerator enumerator = new(reader);
                                    while (enumerator.MoveNext())
                                    {
                                        string? s = enumerator.Current;
                                        if (s is not null && TryGetStation(s, out Station? station))
                                        {
                                            stations.Add(station);
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

#if NET
        private bool TryGetStation(in string id, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Station? station)
#else
        private bool TryGetStation(in string id, out Station station)
#endif
        {
            for (int i = 0; i < this.stations.Length; i++)
            {
                ref Station station1 = ref this.stations[i];
                if (station1.Id == id)
                {
                    station = station1;
                    return true;
                }
            }
            station = default!;
            return false;
        }

    }

}
