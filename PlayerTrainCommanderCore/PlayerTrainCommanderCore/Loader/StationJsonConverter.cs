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
                string? id = null;
                int startDepth = reader.CurrentDepth;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        if (reader.CurrentDepth == startDepth)
                            break;
                    }
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
