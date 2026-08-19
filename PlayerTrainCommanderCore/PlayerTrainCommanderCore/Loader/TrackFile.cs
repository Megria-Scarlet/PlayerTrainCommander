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
    }
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
}
