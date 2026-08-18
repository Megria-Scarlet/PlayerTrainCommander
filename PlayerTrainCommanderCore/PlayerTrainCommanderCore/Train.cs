using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    public class Train
    {
        private string id;
        private string? name;
    }
    public class TrainJsonConverter : JsonConverter<Train>
    {
        public override Train? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Train value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
