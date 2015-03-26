using DocSearcher.Control;
using DocSearcher.Message;
using DocSearcher.Model;
using DocSearcher.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DocSearcher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region UserControlManagement

        public SelectionControl SelectionControl;
        public ResearchControl ResearchControl;
        private UserControl _activeView;

        public UserControl ActiveView
        {
            get
            {
                return _activeView;
            }
            set
            {
                if (_activeView == value)
                    return;
                _activeView = value;
                RaisePropertyChanged("ActiveView");
            }
        }

        #endregion UserControlManagement

        #region ArraysForExtensionsTypes

        public ObservableCollection<Extension> ImageExtensions { get; set; }

        public ObservableCollection<Extension> MusicExtensions { get; set; }

        public ObservableCollection<Extension> DocumentExtensions { get; set; }

        public ObservableCollection<Extension> VideoExtensions { get; set; }

        private void InitExtensionCollections()
        {
            ImageExtensions = new ObservableCollection<Extension>();
            MusicExtensions = new ObservableCollection<Extension>();
            DocumentExtensions = new ObservableCollection<Extension>();
            VideoExtensions = new ObservableCollection<Extension>();
        }

        #endregion ArraysForExtensionsTypes

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

        public long TotalSize
        {
            get { return _totalSize; }
            private set
            {
                _totalSize = value;
                RaisePropertyChanged("TotalSize");
            }
        }

        public long Progress
        {
            get { return _progress; }
            private set
            {
                if (_progress != value)
                {
                    _progress = value;
                    RaisePropertyChanged("Progress");
                }
            }
        }

        public int FilesScanned
        {
            get { return _filesScanned; }
            private set
            {
                if (_filesScanned != value)
                {
                    _filesScanned = value;
                    RaisePropertyChanged("FilesScanned");
                }
            }
        }

        public string ScaningFilePath
        {
            get
            {
                return _scaningFilePath;
            }
            private set
            {
                if (_scaningFilePath != value)
                {
                    _scaningFilePath = value;
                    RaisePropertyChanged("ScaningFilePath");
                }
            }
        }

        #endregion Setters & Getters

        private List<string> _extensions = new List<string>();
        private List<string> _paths = new List<string>();

        private int _filesScanned = 0;
        private int _filesFound = 0;
        private long _totalSize = 0;
        private long _progress = 0;
        private string _scaningFilePath = "";

        public MainViewModel()
        {
            Messenger.Default.Register<MainWindowUidMessage>(this, LoadControls);

            //********* here you are your test fonctions **********

            TotalSize = new DrivesExplorer().GetUsedSpace();

            InitExtensionCollections();

            //Task.Run(() =>
            //{
            //    StartScanning();
            //});

            using (var db = new SQLiteConnection("localDB.sqlite"))
            {
                db.CreateTable<Extension>();

                if (!(db.Table<Extension>().Count() > 0))
                {
                    InsertSomeDatasIntoTable(db);
                }

                var extensions = db.Table<Extension>().ToList();

                foreach (var ext in extensions)
                {
                    switch (ext.Type)
                    {
                        case "image":
                            ImageExtensions.Add(ext);
                            break;

                        case "video":
                            VideoExtensions.Add(ext);
                            break;

                        case "document":
                            DocumentExtensions.Add(ext);
                            break;

                        case "music":
                            MusicExtensions.Add(ext);
                            break;

                        default:
                            break;
                    }
                }
            }

            //*****************************************************
        }

        private void LoadControls(MainWindowUidMessage obj)
        {
            Dispatcher.FromThread(obj.ThreadUid).Invoke(() =>
            {
                SelectionControl = new SelectionControl();
                ResearchControl = new ResearchControl();
            });

            ActiveView = SelectionControl;
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
            ScaningFilePath = "Done :)";
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
                    }

                    ScaningFilePath = file_info.FullName;
                    FilesScanned++;
                    Progress += file_info.Length;
                }
            }
            catch { }
        }

        #region TestSection

        private void InsertSomeDatasIntoTable(SQLiteConnection db)
        {
            db.Insert(new Extension()
            {
                Name = "jpg",
                Type = "image"
            });
            db.Insert(new Extension()
            {
                Name = "png",
                Type = "image"
            });
            db.Insert(new Extension()
            {
                Name = "gif",
                Type = "image"
            });
            db.Insert(new Extension()
            {
                Name = "doc",
                Type = "document"
            });
            db.Insert(new Extension()
            {
                Name = "mp3",
                Type = "music"
            });
            db.Insert(new Extension()
            {
                Name = "flv",
                Type = "video"
            });
        }

        #endregion TestSection
    }
}