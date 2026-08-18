using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    [JsonConverter(typeof(TrainJsonConverter))]
    public class Train
    {
        private string id;
        private string? name;
        private uint seating;
        private uint standing;
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
                JsonElement element1 = element.GetProperty("seat");
                if (!element1.GetProperty("seating").TryGetUInt32(out uint seat))
                    seat = 0;
                if (!element1.GetProperty("standing").TryGetUInt32(out uint stand))
                    stand = 0;
                element1 = element.GetProperty("performance");
                if (!element1.GetProperty("acceleration").TryGetSingle(out float acceleration))
                    acceleration = 0;
                if (!element1.GetProperty("deceleration").TryGetSingle(out float deceleration))
                    deceleration = 0;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Train value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
