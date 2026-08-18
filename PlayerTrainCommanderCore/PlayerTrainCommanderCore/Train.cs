using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    /// <summary>
    /// 車両データを定義するクラス。
    /// </summary>
    [JsonConverter(typeof(TrainJsonConverter))]
    public class Train : IInherentObject
    {
        private string id;
        private string? name;
        private uint seating;
        private uint standing;
        private float acceleration;
        private float deceleration;

        public Train(string id, string? name, uint seating, uint standing, float acceleration, float deceleration)
        {
            this.id = id;
            this.name = name;
            this.seating = seating;
            this.standing = standing;
            this.acceleration = acceleration;
            this.deceleration = deceleration;
        }

        /// <inheritdoc/>
        public string Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return id; }
        }
        /// <inheritdoc cref="Station.Name"/>
        public string? Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.name;
        }

        /// <summary>
        /// json ファイルのストリームからデータを読み取り、 <see cref="Train"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <param name="utf8Json">json ファイルのストリーム。</param>
        /// <returns>json ファイルから読み取られた <see cref="Train"/> 型のオブジェクト。</returns>
        /// <param name="options">デシリアライズに使用するオプション。</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Train? FromJson(Stream utf8Json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<Train>(utf8Json, options);
        }
    }
    /// <summary>
    /// <see cref="Train"/> 型のオブジェクトと Json ファイルへの相互変換機能を提供します。
    /// </summary>
    public class TrainJsonConverter : JsonConverter<Train>
    {
        /// <inheritdoc/>
        public override Train? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(Train).IsAssignableFrom(typeToConvert))
            {
                JsonElement element = JsonElement.ParseValue(ref reader);
                string ver = element.GetProperty("version").ToString();
                string? name = element.GetProperty("name").ToString();
                string? id = element.GetProperty("id").ToString();
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
                return new Train(id, name, seat, stand, acceleration, deceleration);
            }
            return null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Train value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
