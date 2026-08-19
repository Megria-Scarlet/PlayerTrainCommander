using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// 線路を管理するクラス。
    /// </summary>
    public class Track
    {
        /// <summary>
        /// 上り方面と下り方面の接続先を管理する基本的なクラス。
        /// </summary>
        [JsonConverter(typeof(LinkerJsonConverter))]
        public struct Linker : IEquatable<Linker>
        {
            internal string[]? upIds;
            internal string[]? downIds;

            #region コンストラクタ
            /// <summary>
            /// 上り方面と下り方面の接続先を示す文字列型の配列を指定して、新しい <see cref="Linker"/> 型のオブジェクトを作成します。
            /// </summary>
            /// <param name="upIds">上り方面の接続先を示す文字列型の配列。</param>
            /// <param name="downIds">下り方面の接続先を示す文字列型の配列。</param>
            /// <remarks>
            /// 指定した配列をシャローコピーした配列を内部で使用します。
            /// </remarks>
            public Linker(string[]? upIds, string[]? downIds)
            {
                if (upIds is not null)
                    this.upIds = (string[])upIds.Clone();
                if (downIds is not null)
                    this.downIds = (string[])downIds.Clone();
            }
            /// <summary>
            /// 上り方面と下り方面の接続先を示す文字列型のコレクションを指定して、新しい <see cref="Linker"/> 型のオブジェクトを作成します。
            /// </summary>
            /// <param name="upIds">上り方面の接続先を示す文字列型のコレクション。</param>
            /// <param name="downIds">下り方面の接続先を示す文字列型のコレクション。</param>
            public Linker(IEnumerable<string>? upIds, IEnumerable<string>? downIds)
            {
                if (upIds is not null && upIds.Any())
                    this.upIds = [.. upIds];
                if (downIds is not null && downIds.Any())
                    this.downIds = [.. downIds];
            }
            /// <summary>
            /// 上り方面と下り方面の接続先を示す文字列型のスパンを指定して、新しい <see cref="Linker"/> 型のオブジェクトを作成します。
            /// </summary>
            /// <param name="upIds">上り方面の接続先を示す文字列型のスパン。</param>
            /// <param name="downIds">下り方面の接続先を示す文字列型のスパン。</param>
            public Linker(scoped ReadOnlySpan<string> upIds, scoped ReadOnlySpan<string> downIds)
            {
                if (!upIds.IsEmpty)
                    this.upIds = upIds.ToArray();
                if (!downIds.IsEmpty)
                    this.downIds = downIds.ToArray();
            }
            #endregion


            /// <inheritdoc/>
            public override readonly bool Equals(object? obj)
            {
                return obj is Linker linker && Equals(linker);
            }
            /// <inheritdoc/>
            public readonly bool Equals(Linker other)
            {
                if (Equals(upIds, other.upIds) && Equals(downIds, other.downIds))
                    return true;
                else
                    return false;

                static bool Equals(string[]? array0, string[]? array1)
                {
                    if (array0 is null)
                    {
                        return array1 is null;
                    }
                    else if (array1 is null)
                    {
                        return false;
                    }
                    else
                    {
#if NET
                        return ReferenceEquals(array0, array1) || array0.AsSpan().SequenceEqual(array1);
#else
                        return ReferenceEquals(array0, array1) || array0.SequenceEqual(array1);
#endif
                    }
                }
            }

            /// <inheritdoc/>
            public override readonly int GetHashCode()
            {
#if NET
                return HashCode.Combine(upIds, downIds);
#else
                int hashCode = -172096013;
                hashCode = hashCode * -1521134295 + EqualityComparer<string[]?>.Default.GetHashCode(upIds!);
                hashCode = hashCode * -1521134295 + EqualityComparer<string[]?>.Default.GetHashCode(downIds!);
                return hashCode;
#endif
            }
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(Linker left, Linker right)
            {
                return left.Equals(right);
            }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static bool operator !=(Linker left, Linker right)
            {
                return !left.Equals(right);
            }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
        }

        /// <summary>
        /// <see cref="Linker"/> 型のオブジェクトと Json ファイルへの相互変換機能を提供します。
        /// </summary>
        public class LinkerJsonConverter : JsonConverter<Linker>
        {
            const string UpIdsProperty = "joinupids";
            const string DownIdsProperty = "joindownids";
            /// <inheritdoc/>
            public override Linker Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (typeof(Linker) == typeToConvert)
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        JsonElement element = JsonElement.ParseValue(ref reader);
                        List<string>? upIds, downIds;
                        if (element.TryGetProperty(UpIdsProperty, out JsonElement element1))
                        {
                            upIds = CreateList(element1);
                        }
                        else
                        {
                            upIds = null;
                        }
                        if (element.TryGetProperty(DownIdsProperty, out element1))
                        {
                            downIds = CreateList(element1);
                        }
                        else
                        {
                            downIds = null;
                        }
#if NET
                        return new Linker(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(upIds), System.Runtime.InteropServices.CollectionsMarshal.AsSpan(downIds));
#else
                        return new Linker(upIds, downIds);
#endif
                    }
                }
                return default;
            }
            /// <inheritdoc/>
            public override void Write(Utf8JsonWriter writer, Linker value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                if (value.upIds is not null)
                {
                    if (value.upIds.Length == 1)
                    {
                        writer.WriteString(UpIdsProperty, value.upIds[0]);
                    }
                    else
                    {
                        writer.WriteStartArray(UpIdsProperty);
                        foreach (var id in value.upIds)
                        {
                            writer.WriteStringValue(id);
                        }
                        writer.WriteEndArray();
                    }
                }
                if (value.downIds is not null)
                {
                    if (value.downIds.Length == 1)
                    {
                        writer.WriteString(DownIdsProperty, value.downIds[0]);
                    }
                    else
                    {
                        writer.WriteStartArray(DownIdsProperty);
                        foreach (var id in value.downIds)
                        {
                            writer.WriteStringValue(id);
                        }
                        writer.WriteEndArray();
                    }
                }
            }

            private static List<string>? CreateList(JsonElement element)
            {
                if (element.ValueKind is JsonValueKind.Null)
                {
                    return null;
                }
                else if (element.ValueKind == JsonValueKind.Array)
                {
                    List<string> list = new(element.GetArrayLength() + 4);
                    JsonElement.ArrayEnumerator enumerator = element.EnumerateArray();
                    while (enumerator.MoveNext())
                    {
                        if (TryGetString(enumerator.Current, out string value))
                        {
                            list.Add(value);
                        }
                    }
                    return list.Count > 0 ? list : null;
                }
                else
                {
                    return TryGetString(element, out string value) ? ([value]) : null;
                }

                static bool TryGetString(in JsonElement element, out string value)
                {
                    value = element.GetString()!;
                    return !string.IsNullOrWhiteSpace(value);
                }
            }
        }
    }
}
