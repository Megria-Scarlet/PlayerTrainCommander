using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
    public class CoreLoader
    {
        private DirectoryInfo rootDirectory;

        public CoreLoader(DirectoryInfo rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public void Load(ILoaderCallback callback)
        {
            try
            {

            }
            catch (Exception ex)
            {
                callback.WriteError("An exception occurred.", ex);
            }
        }
    }
}
