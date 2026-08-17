using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// 列車種別を管理するクラス。
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class ServiceType
    {
        private string id;
        private string? name;
        private string? abbreviation;
        private string[] stopStationIds;
        private Station[] stopStations;

        /// <summary>
        /// 固有の ID と表示名を指定して、新しい <see cref="ServiceType"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="id">固有の ID 。</param>
        /// <param name="name">識別に使用する任意の文字列。</param>
        /// <param name="abbreviation">識別に使用する任意の省略文字列。</param>
        /// <param name="stopStations">停車駅の Id を列挙するオブジェクト。</param>
        public ServiceType(string id, string? name, string? abbreviation, IEnumerable<string> stopStations)
        {
            this.id = id;
            this.name = name ?? abbreviation;
            this.abbreviation = abbreviation;
            this.stopStationIds = [..stopStations];
            this.stopStations = [];
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
        /// 停車駅を取得します。
        /// </summary>
        /// <returns>停車駅を示す読み取り専用のスパン。</returns>
        public ReadOnlySpan<Station> StopStations
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.stopStations;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay()
        {
            return name ?? ToString()!;
        }
    }
}
