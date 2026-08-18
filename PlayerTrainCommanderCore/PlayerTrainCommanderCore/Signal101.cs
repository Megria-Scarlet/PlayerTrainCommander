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
    /// 信号を定義します。
    /// </summary>
    public class Signal101 : IInherentObject
    {
        private string id;
        private string? name;

        private View[] views;
        private uint[] speedLimits;
        /// <summary>
        /// 値を指定して、新しい <see cref="Signal101"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="id">固有の ID 。</param>
        /// <param name="name">識別に使用する任意の文字列。</param>
        /// <param name="views"></param>
        /// <param name="speedLimits"></param>
        public Signal101(string id, string? name, IEnumerable<View> views, IEnumerable<uint> speedLimits)
        {
            this.id = id;
            this.name = name;
            this.views = [.. views];
            this.speedLimits = [.. speedLimits];
        }

        /// <inheritdoc/>
        public string Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => id;
        }
        /// <inheritdoc cref="Station.Name"/>
        public string? Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => name;
        }
        /// <summary>
        /// 速度制限を格納する読み取り専用のスパンを取得します。
        /// </summary>
        /// <returns>速度制限を格納する読み取り専用のスパン。</returns>
        public ReadOnlySpan<uint> SpeedLimits
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => speedLimits;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetSpeedLimit(int signalLevel)
        {
            return signalLevel < speedLimits.Length ? speedLimits[signalLevel] : uint.MaxValue;
        }

        [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
        public class View
        {
            private string texturePath;
            public View(string texturePath)
            {
                this.texturePath = texturePath;
            }
            /// <summary>
            /// 画像ファイルパスを取得します。
            /// </summary>
            /// <returns>相対画像ファイルパス。</returns>
            public string TexturePath
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => texturePath;
            }

            private string GetDebuggerDisplay()
            {
                return string.IsNullOrWhiteSpace(texturePath) ? ToString()! : texturePath;
            }
        }
    }
}
