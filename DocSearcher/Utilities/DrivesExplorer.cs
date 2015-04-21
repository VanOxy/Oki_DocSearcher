using DocSearcher.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DocSearcher.Utilities
{
    internal class DrivesExplorer
    {
        private static long _spaceOccupiedByFolder;

        /// <summary>
        /// Returns total amount of space occupied by files on drives
        /// passed into parameter as an collection.
        /// </summary>
        /// <param name="drives"></param>
        /// <returns></returns>
        public static long GetUsedSpace_Drives(Collection<Drive> drives)
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

        public static long GetSpace_Folder(string path)
        {
            _spaceOccupiedByFolder = 0;

            RecursiveSearchFilesIntoDirectory(new DirectoryInfo(path));

            return _spaceOccupiedByFolder;
        }

        private static void RecursiveSearchFilesIntoDirectory(DirectoryInfo dir_info)
        {
            try
            {
                foreach (var subdir_info in dir_info.GetDirectories())
                    RecursiveSearchFilesIntoDirectory(subdir_info);
            }
            catch { }

            try
            {
                foreach (FileInfo file_info in dir_info.GetFiles())
                    _spaceOccupiedByFolder += file_info.Length;
            }
            catch { }
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