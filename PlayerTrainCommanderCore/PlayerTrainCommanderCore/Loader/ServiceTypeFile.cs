using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
    [JsonConverter(typeof(ServiceTypeFileJsonConverter))]
    public class ServiceTypeFile
    {
        private string version;
        private ServiceType[] serviceTypes;
        public ServiceTypeFile(string version, IEnumerable<ServiceType> serviceTypes)
        {
            this.serviceTypes = [.. serviceTypes];
        }
        public ServiceTypeFile(string version, scoped ReadOnlySpan<ServiceType> serviceTypes)
        {
            this.serviceTypes = serviceTypes.ToArray();
        }

        public ReadOnlySpan<ServiceType> ServiceTypes
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => serviceTypes;
        }


        /// <summary>
        /// json ファイルのストリームからデータを読み取り、 <see cref="ServiceTypeFile"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <param name="utf8Json">json ファイルのストリーム。</param>
        /// <param name="stations">駅リスト。</param>
        /// <param name="options">デシリアライズに使用するオプション。</param>
        /// <returns>json ファイルから読み取られた <see cref="ServiceTypeFile"/> 型のオブジェクト。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ServiceTypeFile? FromJson(System.IO.Stream utf8Json, IEnumerable<Station> stations, JsonSerializerOptions? options = null)
        {
            ServiceTypeJsonConverter serviceTypeJsonConverter = new(stations);
            options ??= new JsonSerializerOptions();
            options.Converters.Add(serviceTypeJsonConverter);
            return JsonSerializer.Deserialize<ServiceTypeFile>(utf8Json, options);
        }
        /// <inheritdoc cref="FromJson(System.IO.Stream, IEnumerable{Station}, JsonSerializerOptions?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ServiceTypeFile? FromJson(System.IO.Stream utf8Json, scoped ReadOnlySpan<Station> stations, JsonSerializerOptions? options = null)
        {
            ServiceTypeJsonConverter serviceTypeJsonConverter = new(stations);
            options ??= new JsonSerializerOptions();
            options.Converters.Add(serviceTypeJsonConverter);
            return JsonSerializer.Deserialize<ServiceTypeFile>(utf8Json, options);
        }
    }
    public class ServiceTypeFileJsonConverter : JsonConverter<ServiceTypeFile>
    {
        public override ServiceTypeFile? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(ServiceTypeFile).IsAssignableFrom(typeToConvert))
            {
                string? ver = null;
                List<ServiceType> services = new(16);
                int startDepth = reader.CurrentDepth;

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
                        else if (reader.ValueTextEquals("types"))
                        {
                            reader.Read();
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                reader.Read();
                                
                                do
                                {
                                    ServiceType? station = JsonSerializer.Deserialize<ServiceType>(ref reader, options);
                                    if (station is not null)
                                        services.Add(station);
                                    else
                                        throw new JsonException("ServiceType 型のデシリアライズに失敗しました。");
                                    reader.Read();
                                }
                                while (reader.TokenType != JsonTokenType.EndArray);
                            }
                        }
                    }
                }
                if (ver is not null)
                    return new ServiceTypeFile(ver, services);
                else
                    throw new JsonException("プロパティ \"version\" が見つかりませんでした。");
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, ServiceTypeFile value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
}
