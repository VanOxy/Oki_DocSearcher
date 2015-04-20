using Avalon.Windows.Dialogs;
using DocSearcher.Control;
using DocSearcher.Message;
using DocSearcher.Model;
using DocSearcher.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DocSearcher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region UserControlManagement

        public SelectionControl SelectionControl;
        public ResearchControl ResearchControl;
        public ExtensionsManagementControl ExtensionsManagementControl;
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

        #region SelectionMode

        private string connectionString = "localDB.sqlite";

        #region ArraysForExtensionsTypes

        public List<string> FileTypes { get; set; }

        public ObservableCollection<string> FileTypeExtensionsList { get; set; }

        public ObservableCollection<Extension> ImageExtensions { get; set; }

        public ObservableCollection<Extension> MusicExtensions { get; set; }

        public ObservableCollection<Extension> DocumentExtensions { get; set; }

        public ObservableCollection<Extension> VideoExtensions { get; set; }

        public ObservableCollection<Drive> Drives { get; set; }

        private void InitExtensionCollections()
        {
            ImageExtensions = new ObservableCollection<Extension>();
            MusicExtensions = new ObservableCollection<Extension>();
            DocumentExtensions = new ObservableCollection<Extension>();
            VideoExtensions = new ObservableCollection<Extension>();
            Drives = new ObservableCollection<Drive>();
        }

        private void InitFileTypes()
        {
            FileTypeExtensionsList = new ObservableCollection<string>();
            FileTypes = new List<string>();
            FileTypes.Add("Documents");
            FileTypes.Add("Video");
            FileTypes.Add("Images");
            FileTypes.Add("Music");
        }

        private void UpdateManagementExtensionsList(ExtensionManagementFileTypeSelectedMessage message)
        {
            var fileType = NormalizeFileTypeString(message.SelectedFileType);

            using (var db = new SQLiteConnection(connectionString))
            {
                var extensions = db.Table<Extension>().Where(file => file.Type.Equals(fileType)).ToList();
                FileTypeExtensionsList.Clear();
                foreach (var ext in extensions)
                {
                    FileTypeExtensionsList.Add(ext.Name);
                }
            }
        }

        #endregion ArraysForExtensionsTypes

        #region Commands & Realisation

        public RelayCommand ManageExtensionsCommand { get; set; }

        public RelayCommand AcceedSelectionControlCommand { get; set; }

        public RelayCommand<string> AddExtensionCommand { get; set; }

        public RelayCommand<string> RemoveExtensionCommand { get; set; }

        public RelayCommand SelectFolderCommand { get; set; }

        public RelayCommand StartSearchCommand { get; set; }

        private void InitCommands()
        {
            ManageExtensionsCommand = new RelayCommand(ManageExtensions);
            AcceedSelectionControlCommand = new RelayCommand(AcceedSelectionControl);
            AddExtensionCommand = new RelayCommand<string>(AddExtension);
            RemoveExtensionCommand = new RelayCommand<string>(DeleteExtension);
            SelectFolderCommand = new RelayCommand(SelectFolder);
            StartSearchCommand = new RelayCommand(PrepareAndStartScan);
        }

        private void AcceedSelectionControl()
        {
            ActiveView = SelectionControl;
        }

        private void ManageExtensions()
        {
            ActiveView = ExtensionsManagementControl;
        }

        private void AddExtension(string str)
        {
            try
            {
                var extension = ExtensionToAdd;
                if (String.IsNullOrEmpty(extension))
                    return;

                ExtensionToAdd = "";
                var fileType = NormalizeFileTypeString(str);

                // add to database
                using (var db = new SQLiteConnection(connectionString))
                {
                    db.Insert(new Extension()
                    {
                        Name = extension,
                        Type = fileType
                    });
                }

                // add to the ListView
                FileTypeExtensionsList.Add(extension);
                RefreshExtensionLists();
            }
            catch { }
        }

        private void DeleteExtension(string selectedExtension)
        {
            if (String.IsNullOrEmpty(selectedExtension))
                return;

            // delete from DataBase
            using (var db = new SQLiteConnection(connectionString))
            {
                var obj = db.Table<Extension>().Where(ext => ext.Name.Equals(selectedExtension)).First();
                db.Delete<Extension>(obj.Id);
            }

            // refresh all Lists
            RefreshExtensionLists();
            FileTypeExtensionsList.Remove(selectedExtension);
        }

        private void SelectFolder()
        {
            var dialog = new FolderBrowserDialog();
            dialog.BrowseFiles = false;

            // if folder selected
            if ((bool)dialog.ShowDialog())
            {
                // clear selected drives
                foreach (var item in Drives)
                    item.Checked = false;

                // attribute path to the
                ScanningFilePath = dialog.SelectedPath;
            }
        }

        #endregion Commands & Realisation

        #region ExtensionsManagement

        private string _extensionToAdd;

        public string ExtensionToAdd
        {
            get { return _extensionToAdd; }
            set
            {
                if (_extensionToAdd == value)
                    return;
                _extensionToAdd = value;
                RaisePropertyChanged("ExtensionToAdd");
            }
        }

        #endregion ExtensionsManagement

        #region Tools

        private string NormalizeFileTypeString(string str)
        {
            var fileType = str.Trim().ToLower();

            if (fileType.Equals("documents") || fileType.Equals("images"))
                fileType = fileType.Remove(fileType.Length - 1);

            return fileType;
        }

        /// <summary>
        /// Fill the Database with hardcoded usual extensions.
        /// </summary>
        private void InitExtensions()
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.CreateTable<Extension>();

                if (!(db.Table<Extension>().Count() > 0))
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
            }
        }

        private void FillExtensionsListsFromDatabase()
        {
            using (var db = new SQLiteConnection(connectionString))
            {
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
        }

        private void ClearExtensionsLists()
        {
            ImageExtensions.Clear();
            VideoExtensions.Clear();
            DocumentExtensions.Clear();
            MusicExtensions.Clear();
        }

        private void RefreshExtensionLists()
        {
            ClearExtensionsLists();
            FillExtensionsListsFromDatabase();
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<MainWindowUidMessage>(this, LoadControls);
            Messenger.Default.Register<ExtensionManagementFileTypeSelectedMessage>
                (this, UpdateManagementExtensionsList);
            Messenger.Default.Register<ClearScaningFilePathMessage>(this, ClearScaningFilePath);
        }

        private void ClearScaningFilePath(Message.ClearScaningFilePathMessage obj)
        {
            ScanningFilePath = "";
        }

        private void LoadControls(MainWindowUidMessage obj)
        {
            Dispatcher.FromThread(obj.ThreadUid).Invoke(() =>
            {
                SelectionControl = new SelectionControl();
                ResearchControl = new ResearchControl();
                ExtensionsManagementControl = new ExtensionsManagementControl();
            });

            ActiveView = SelectionControl;
        }

        #endregion Tools

        #endregion SelectionMode

        #region FilesResearchMode

        private List<string> _extensions = new List<string>();
        private List<string> _paths = new List<string>();

        #region Getters&Setters for VM

        private int _filesFound = 0;

        public int FilesFound
        {
            get { return _filesFound; }
            private set
            {
                _filesFound = value;
                RaisePropertyChanged("FilesFound");
            }
        }

        private long _totalSizeToScan = 10;

        public long TotalSizeToScan
        {
            get { return _totalSizeToScan; }
            private set
            {
                _totalSizeToScan = value;
                RaisePropertyChanged("TotalSizeToScan");
            }
        }

        private long _progress = 0;

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

        private int _filesScanned = 0;

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

        private string _scanningFilePath = "";

        public string ScanningFilePath
        {
            get
            {
                return _scanningFilePath;
            }
            private set
            {
                if (_scanningFilePath != value)
                {
                    _scanningFilePath = value;
                    RaisePropertyChanged("ScanningFilePath");
                }
            }
        }

        #endregion Getters&Setters for VM

        private void PrepareAndStartScan()
        {
            // check for extensions
            GetSelectedExtensions();
            if (_extensions.Count == 0)
            {
                MessageBox.Show("You did not select any extension.\nPlease, select one extension at least.");
                return;
            }

            // check for drives or folder
            var result = CheckFolderSelection();
            if (result == false)
            {
                MessageBox.Show("You did not select any drive/dirrectory where research should be performed.\n" +
                                "Please, select at least one drive or folder.");
                return;
            }

            InitStockObjects();

            ActiveView = ResearchControl;
            Messenger.Default.Send(new ChangeWindowSizeMessage("research"));

            Scan();

            // todo --> create Modern Charts page, bindings
            //ActiveView = ChartsControl;

            Messenger.Default.Send(new ChangeWindowSizeMessage("stat"));
        }

        private async void Scan()
        {
            // if drives are selected
            if (ScanningFilePath == "")
            {
                TotalSizeToScan = DrivesExplorer.GetUsedSpace_Drives(Drives);

                await Task.Run(() =>
                {
                    foreach (var drive in Drives)
                    {
                        if (drive.Checked)
                            RecursiveSearchFilesIntoDirectory(new DirectoryInfo(drive.Name));
                    }
                    ScanningFilePath = "Done.. :)";
                });
            }
            // if just folder selected
            else
            {
                var folder = ScanningFilePath;

                await Task.Run(() =>
                {
                    ScanningFilePath = "Preparing to analyse...";
                    TotalSizeToScan = DrivesExplorer.GetSpace_Folder(folder);
                });

                if (TotalSizeToScan < 0)
                {
                    MessageBox.Show("In order to perform research into selected directory you need to launch application with administrator privileges.\n" +
                                    "Please restart the application as administrator.");
                    ScanningFilePath = "Error...!";
                    return;
                }

                await Task.Run(() =>
                {
                    RecursiveSearchFilesIntoDirectory(new DirectoryInfo(folder));
                    ScanningFilePath = "Done.. :)";
                });
            }
        }

        private void RecursiveSearchFilesIntoDirectory(DirectoryInfo dir_info)
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
                {
                    // modify progress view
                    ScanningFilePath = file_info.FullName;
                    FilesScanned++;
                    Progress += file_info.Length;

                    // if convinient extension, get info
                    if (_extensions.Any(ext => ext == file_info.Extension))
                    {
                        RecoltInformations(file_info);
                        FilesFound++;
                    }
                }
            }
            catch { }
        }

        private void RecoltInformations(FileInfo file_info)
        {
            _paths.Add(file_info.FullName);

            //if(file_info.Extension == )
        }

        #region Tools

        private void GetSelectedExtensions()
        {
            CheckExtensions(ImageExtensions);
            CheckExtensions(MusicExtensions);
            CheckExtensions(DocumentExtensions);
            CheckExtensions(VideoExtensions);
        }

        private void CheckExtensions(ObservableCollection<Extension> list)
        {
            foreach (var item in list)
                if (item.Checked)
                    _extensions.Add("." + item.Name);
        }

        private bool CheckFolderSelection()
        {
            bool flag = false;

            foreach (var item in Drives)
            {
                if (item.Checked)
                    flag = true;
            }

            if (ScanningFilePath == "" && flag == false)
                return false;

            return true;
        }

        private void InitStockObjects()
        {
            InitFor(ImageExtensions);
            InitFor(MusicExtensions);
            InitFor(DocumentExtensions);
            InitFor(VideoExtensions);
        }

        private void InitFor(ObservableCollection<Extension> collection)
        {
            bool flag = false;
            foreach (var item in collection)
            {
                if (item.Checked)
                {
                    if (flag)
                        return;

                    Stats.Add(new DocTypeCollection(item.Type));
                    flag = true;
                }
            }
        }

        #endregion Tools

        #endregion FilesResearchMode

        #region ChartsMode

        // stock objects
        public ObservableCollection<DocTypeCollection> Stats =
            new ObservableCollection<DocTypeCollection>();

        #endregion ChartsMode

        public MainViewModel()
        {
            RegisterMessages();
            InitCommands();
            InitExtensionCollections();
            InitFileTypes();
            InitExtensions();
            FillExtensionsListsFromDatabase();

            Drives = DrivesExplorer.GetDrives();
        }
    }
}