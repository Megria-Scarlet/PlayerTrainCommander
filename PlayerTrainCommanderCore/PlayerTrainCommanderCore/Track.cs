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
        public struct Linker
        {
            string[]? upIds;
            string[]? downIds;

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
        }
    }
}
