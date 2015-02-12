using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DocSearcher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string[] _extensions = { ".pdf", ".doc", ".docx" };
        private List<string> _paths = new List<string>();

        private int _filesFound = 0;
        private int _copyProgress;
        private int _totalSize = 0;
        private int _sizeToDl;

        #region Setters & Getters

        public int FilesFound
        {
            get { return _filesFound; }
            private set
            { 
                _filesFound = value;
                RaisePropertyChanged("FilesFound"); 
            }
        }

        #endregion Setters & Getters

        public MainViewModel()
        {
            Task.Run(() =>
            {
                StartScanning();
            });
            
        }

        private void StartScanning()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    ExploreDrive(drive.Name);
                }
            }
        }

        private void ExploreDrive(string driveName)
        {
            DirectoryInfo dir_info = new DirectoryInfo(driveName);
            SearchFiles(dir_info, _paths);
        }

        /// <summary>
        /// Take into parameters directory name and List to fill.
        /// Fill the last one with paths of found files.
        /// </summary>
        /// <param name="dir_info"></param>
        /// <param name="file_list"></param>
        private void SearchFiles(DirectoryInfo dir_info, List<string> file_list)
        {
            try
            {
                foreach (DirectoryInfo subdir_info in dir_info.GetDirectories())
                {
                    SearchFiles(subdir_info, file_list);
                }
            }
            catch { }
            try
            {
                foreach (FileInfo file_info in dir_info.GetFiles())
                {
                    if (_extensions.Any(ext => ext == file_info.Extension))
                    {
                        file_list.Add(file_info.FullName);
                        FilesFound++;
                        /// todo
                        /// add file weight into _totalSize
                        /// like this
                        /// _totalSize += file_info.Lenght;
                    }
                }
            }
            catch { }
        }
    }
}