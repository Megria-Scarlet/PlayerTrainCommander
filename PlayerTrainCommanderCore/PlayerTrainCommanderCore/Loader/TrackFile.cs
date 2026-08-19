using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
    public class TrackFile
    {
        private string version;
        private Track[] tracks;
        public TrackFile(string version, Track[] tracks)
        {
            this.version = version;
            this.tracks = tracks;
        }
        /// <summary>
        /// <see cref="Track"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <returns>読み取られた <see cref="Track"/> 型のオブジェクト。</returns>
        public ReadOnlySpan<Track> Tracks
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => tracks;
        }
        /// <summary>
        /// バージョンを示す文字列を取得します。
        /// </summary>
        /// <returns>バージョンを示す文字列。</returns>
        public string Version
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => version;
        }
    }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
}
