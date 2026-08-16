using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// 駅情報を管理するクラス。
    /// </summary>
    public class Station : IInherentObject
    {
        private string id;
        private string? name;
        private string? abbreviation;

        /// <summary>
        /// 固有の ID と表示名を指定して、新しい <see cref="Station"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="id">固有の ID 。</param>
        /// <param name="name">識別に使用する任意の文字列。</param>
        /// <param name="abbreviation">識別に使用する任意の省略文字列。</param>
        public Station(string id, string? name, string? abbreviation)
        {
            this.id = id;
            this.name = name;
            this.abbreviation = abbreviation;
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
    }
}
