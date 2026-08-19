using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public class BaseLinker
        {
            protected string[]? upIds;
            protected string[]? downIds;
        }
    }
}
