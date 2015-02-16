using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocSearcher.Utilities
{
    class DrivesExplorer
    {
        /// <summary>
        /// Function returns total amound of space ocuped by files on all drives.
        /// </summary>
        /// <returns></returns>
        public long GetUsedSpace()
        {
            long usedSpace = 0;
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    usedSpace += drive.TotalSize - drive.TotalFreeSpace;
                }
            }
            return usedSpace;
        }
    }
}
