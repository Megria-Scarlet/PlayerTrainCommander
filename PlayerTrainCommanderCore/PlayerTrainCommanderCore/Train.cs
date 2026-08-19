using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PTC.Core
{
    /// <summary>
    /// 編成データを定義するクラス。
    /// </summary>
    [JsonConverter(typeof(TrainJsonConverter))]
    [System.Diagnostics.DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Train : IInherentObject
    {
        private string id;
        private string? name;
        private uint seating;
        private uint standing;
        private float acceleration;
        private float deceleration;
        private uint totalLength;
        private uint carCount;

        /// <summary>
        /// 値を指定して、新しい <see cref="Train"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="id">固有の ID 。</param>
        /// <param name="name">識別に使用する任意の文字列。</param>
        /// <param name="totalLength">編成長。(mm)</param>
        /// <param name="carCount">両数。</param>
        /// <param name="seating">着席定員数。</param>
        /// <param name="standing">立席定員数。</param>
        /// <param name="acceleration">加速度。(km/h/s)</param>
        /// <param name="deceleration">減速度。(km/h/s)</param>
        public Train(string id, string? name, uint totalLength, uint carCount, uint seating, uint standing, float acceleration, float deceleration)
        {
            this.id = id;
            this.name = name;
            this.totalLength = totalLength;
            this.seating = seating;
            this.standing = standing;
            this.acceleration = acceleration;
            this.deceleration = deceleration;
            this.carCount = carCount;
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
        /// 着席定員数を取得します。
        /// </summary>
        /// <returns>着席定員数を示す 32 ビット符号なし整数。</returns>
        public uint SeatCapacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => seating;
        }
        /// <summary>
        /// 立席定員数を取得します。
        /// </summary>
        /// <returns>立席定員数を示す 32 ビット符号なし整数。</returns>
        public uint StandCapacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => standing;
        }
        /// <summary>
        /// 加速度 (km/h/s) を取得します。
        /// </summary>
        /// <returns>加速度 (km/h/s) を示す単精度浮動小数点数。</returns>
        public float Acceleration
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => acceleration;
        }
        /// <summary>
        /// 減速度 (km/h/s) を取得します。
        /// </summary>
        /// <returns>減速度 (km/h/s) を示す単精度浮動小数点数。</returns>
        public float Deceleration
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => deceleration;
        }

        /// <summary>
        /// 編成長 (mm) を取得します。
        /// </summary>
        /// <returns>編成長 (mm) を示す 32 ビット符号なし整数。</returns>
        public uint TotalLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => totalLength;
        }
        /// <summary>
        /// 編成両数を取得します。
        /// </summary>
        /// <returns>編成両数を示す 32 ビット符号なし整数。</returns>
        public uint CarCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => carCount;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay()
        {
            return name ?? (string.IsNullOrWhiteSpace(id) ? ToString()! : id);
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
                string? ver;

                if (element.TryGetProperty("version", out JsonElement element1) && (ver = element1.GetString()) == "1.0.1")
                {
                    string? name = element.GetProperty("name").ToString();
                    string? id = element.GetProperty("id").ToString();

                    if (!element.TryGetProperty("totallength", out element1) || !element1.TryGetUInt32(out uint totallength))
                        totallength = 0;
                    if (!element.TryGetProperty("cars", out element1) || !element1.TryGetUInt32(out uint cars))
                        cars = 0;
                    uint seat, stand;
                    if (element.TryGetProperty("seat", out element1))
                    {
                        if (!element1.TryGetProperty("seating", out JsonElement element2) || !element2.TryGetUInt32(out seat))
                        {
                            seat = 0;
                        }
                        if (!element1.TryGetProperty("standing", out element2) || !element2.TryGetUInt32(out stand))
                        {
                            stand = 0;
                        }
                    }
                    else
                    {
                        seat = 0;
                        stand = 0;
                    }
                    float acceleration, deceleration;
                    if (element.TryGetProperty("performance", out element1))
                    {
                        if (!element1.TryGetProperty("acceleration", out JsonElement element2) || !element2.TryGetSingle(out acceleration))
                        {
                            acceleration = 0;
                        }
                        if (!element1.TryGetProperty("deceleration", out element2) || !element2.TryGetSingle(out deceleration))
                        {
                            deceleration = 0;
                        }
                    }
                    else
                    {
                        acceleration = 0;
                        deceleration = 0;
                    }
                    return new Train(id, name, totallength, cars, seat, stand, acceleration, deceleration);
                }
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
