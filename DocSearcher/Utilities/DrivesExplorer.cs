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
        /// Returns total amount of space occupied by files on drives
        /// passed into parameter as an collection.
        /// </summary>
        /// <param name="drives"></param>
        /// <returns></returns>
        public static long GetUsedSpaceByDrives(Collection<Drive> drives)
        {
            long usedSpace = 0;
            DriveInfo[] allDrivesInfoCollection = DriveInfo.GetDrives();

            var driveNamesCollection = (from drive in drives
                                        where drive.Checked
                                        select drive.Name).ToList();

            var neededDrivesInfoCollection = (from drive in allDrivesInfoCollection
                                              where driveNamesCollection.Contains(drive.Name)
                                              select drive).ToList();

            foreach (var drive in neededDrivesInfoCollection)
            {
                usedSpace += drive.TotalSize - drive.TotalFreeSpace;
            }

            return usedSpace;
        }

        public static long GetUsedSpaceByFolder(string path)
        {
            string[] a = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            // Calculate total bytes of all files in a loop.
            long b = 0;
            foreach (string name in a)
            {
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
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