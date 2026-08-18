using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Collections;

namespace PTC.Core
{
    /// <summary>
    /// <see cref="Utf8JsonReader"/> から <see cref="object"/> 型を列挙する構造体。
    /// </summary>
    public ref struct JsonObjectEnumerator : IEnumerator<JsonElement>
    {
        private Utf8JsonReader reader;
        private readonly int depth;
        private bool isRead;
        private JsonElement element;
        /// <summary>
        /// <see cref="Utf8JsonReader"/> から <see cref="object"/> 型を列挙するオブジェクトを作成します。
        /// </summary>
        /// <param name="reader"><see cref="Utf8JsonReader"/> 型のオブジェクト。</param>
        /// <remarks>現在の <see cref="Utf8JsonReader.TokenType"/> は <see cref="JsonTokenType.StartObject"/> を示している必要があります。</remarks>
        public JsonObjectEnumerator(Utf8JsonReader reader)
        {
            this.reader = reader;
            this.isRead = false;
            this.depth = reader.CurrentDepth;
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
                    case JsonTokenType.EndObject:
                        if (depth <= reader.CurrentDepth)
                        {
                            return false;
                        }
                        return true;
                    default:
                        element = JsonElement.ParseValue(ref reader);
                        isRead = true;
                        return true;
                }
            }
            return false;
        }
        public bool TryMoveNext(out JsonElement item)
        {
            if (MoveNext())
            {
                item = this.element;
                return true;
            }
            item = default;
            return false;
        }
        /// <summary>
        /// 列挙子の現在位置にある文字列を取得します。
        /// </summary>
        /// <returns>列挙子の現在位置にある文字列。</returns>
        public readonly JsonElement Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetCurrent();
        }
        readonly object? IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetCurrent();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly JsonElement GetCurrent()
        {
            if (!isRead)
                InvalidOperation();
            return element;
        }
        void IDisposable.Dispose()
        {
            reader = default;
        }

        readonly void IEnumerator.Reset()
        {

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
