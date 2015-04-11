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
        //public static long Space { get; set; }

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
            long space = 0;
            try
            {
                string[] a = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

                foreach (string name in a)
                {
                    FileInfo info = new FileInfo(name);
                    space += info.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return space;

            //Space = 0;
            //RecursiveFunction(path);
        }

        //private static void RecursiveFunction(string path)
        //{
        //    try
        //    {
        //        var directories = new DirectoryInfo(path).GetDirectories();
        //        foreach (DirectoryInfo subdirInfo in directories)
        //            RecursiveFunction(subdirInfo.FullName);
        //    }
        //    catch { }

        //    try
        //    {
        //        var files = new DirectoryInfo(path).GetFiles();
        //        foreach (FileInfo fileInfo in files)
        //            Space += fileInfo.Length;
        //    }
        //    catch { }
        //}

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