using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// 列車種別を管理するクラス。
    /// </summary>
    public class ServiceType
    {
        private string id;
        private string? name;

        /// <summary>
        /// 固有の ID と表示名を指定して、新しい <see cref="ServiceType"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="id">固有の ID 。</param>
        /// <param name="name">識別に使用する任意の文字列。</param>
        public ServiceType(string id, string? name)
        {
            this.id = id;
            this.name = name;
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

    }
}
