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
            if (typeof(Train).IsAssignableFrom(typeToConvert))
            {
                JsonElement element = JsonElement.ParseValue(ref reader);
                string ver = element.GetProperty("version").ToString();
                string? name = element.GetProperty("name").ToString();
                if (!element.GetProperty("totallength").TryGetUInt32(out uint totallength))
                    totallength = 0;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Train value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
