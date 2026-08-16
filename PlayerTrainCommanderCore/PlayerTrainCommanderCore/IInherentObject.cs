using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// Id で管理された固有のオブジェクトを表します。
    /// </summary>
    public interface IInherentObject
    {
        /// <summary>
        /// 管理に使用する固有の文字列を取得します。
        /// </summary>
        /// <returns>管理に使用する固有の文字列。</returns>
        public string Id
        {
            get;
        }
    }
}
