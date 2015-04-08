using DocSearcher.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocSearcher.Utilities
{
    internal class DrivesExplorer
    {
        /// <summary>
        /// Function returns total amound of space ocuped by files on all drives.
        /// </summary>
        /// <returns></returns>
        public static long GetUsedSpace()
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

        public static ObservableCollection<Drive> GetDrives()
        {
            var collection = new ObservableCollection<Drive>();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    collection.Add(new Drive()
                    {
                        Name = drive.Name,
                        NameToDisplay = drive.Name + " - " + drive.DriveType.ToString()
                    });
                }
            }

            return collection;
        }
    }
}