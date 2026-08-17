using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PTC.Core
{
    /// <summary>
    /// <see cref="Utf8JsonReader"/> から <see cref="string"/> 型の配列を列挙する構造体。
    /// </summary>
    public ref struct JsonStringArrayEnumerator : IEnumerator<string?>
    {
        private Utf8JsonReader reader;
        private string? current;
        private bool isRead;

        /// <summary>
        /// <see cref="Utf8JsonReader"/> から <see cref="string"/> 型の配列を列挙するオブジェクトを作成します。
        /// </summary>
        /// <param name="reader"><see cref="Utf8JsonReader"/> 型のオブジェクト。</param>
        /// <remarks>現在の <see cref="Utf8JsonReader.TokenType"/> は <see cref="JsonTokenType.StartArray"/> を示している必要があります。</remarks>
        public JsonStringArrayEnumerator(Utf8JsonReader reader)
        {
            this.reader = reader;
            this.isRead = false;
        }

        /// <summary>
        /// 列挙子の現在位置にある文字列を取得します。
        /// </summary>
        /// <returns>列挙子の現在位置にある文字列。</returns>
        public readonly string? Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetCurrent();
        }
        readonly object? IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetCurrent();
        }

        void IDisposable.Dispose()
        {
            reader = default;
        }

        /// <summary>
        /// 列挙子をコレクションの次の要素に進めます。
        /// </summary>
        /// <returns>列挙子が次の要素に正常に進んだ場合は <see langword="true"/>。列挙子がコレクションの末尾を越えた場合は <see langword="false"/>。</returns>
        /// <exception cref="JsonException"></exception>
        public bool MoveNext()
        {
            if (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                    case JsonTokenType.StartArray:
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                    case JsonTokenType.None:
                        throw new JsonException("無効な TokenType です。");
                    case JsonTokenType.EndArray:
                        current = null;
                        return false;
                    default:
                        isRead = true;
                        current = reader.GetString();
                        return true;
                }
            }
            return false;
        }

        readonly void IEnumerator.Reset()
        {
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly string? GetCurrent()
        {
            if (!isRead)
                InvalidOperation();
            return current;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
#if NET
        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
#endif
        private static void InvalidOperation()
        {
            throw new InvalidOperationException();
        }
    }

}
