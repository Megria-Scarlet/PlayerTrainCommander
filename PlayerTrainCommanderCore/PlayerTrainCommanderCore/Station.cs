using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// 駅情報を管理するクラス。
    /// </summary>
    [JsonConverter(typeof(StationJsonConverter))]
#if true
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
#else
    [DebuggerDisplay("{{GetDebuggerDisplay(),nq}}")]
#endif
    public class Station : IInherentObject
    {
        private string id;
        private string? name;
        private string? abbreviation;

        //private string? stationAbbreviation;
        private uint capacity;

        /// <summary>
        /// 固有の ID と表示名を指定して、新しい <see cref="Station"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="id">固有の ID 。</param>
        /// <param name="name">識別に使用する任意の文字列。</param>
        /// <param name="abbreviation">識別に使用する任意の省略文字列。</param>
        /// <param name="capacity">許容乗客数。</param>
        public Station(string id, string? name, string? abbreviation, uint capacity)
        {
            this.id = id;
            this.name = name;
            this.abbreviation = abbreviation;
            this.capacity = capacity;
        }

        /// <summary>
        /// 管理に使用する固有の文字列を取得します。
        /// </summary>
        /// <returns>管理に使用する固有の文字列。</returns>
        public string Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.id;
        }
        /// <summary>
        /// 識別に使用する文字列を取得します。
        /// </summary>
        /// <returns>識別に使用する固有の文字列。</returns>
        public string? Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.name;
        }
        /// <summary>
        /// 識別に使用する省略文字列を取得します。
        /// </summary>
        /// <returns>識別に使用する固有の省略文字列。</returns>
        public string? Abbreviation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.abbreviation;
        }
        /// <summary>
        /// 許容乗客数を取得します。
        /// </summary>
        /// <returns>許容乗客数を表す 32 ビット符号なし整数。</returns>
        public uint Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.capacity;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay()
        {
            return name ?? (string.IsNullOrWhiteSpace(id) ? ToString()! : id);
        }
    }
}
