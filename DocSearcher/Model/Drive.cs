using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DocSearcher.Model
{
    public class Drive : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string NameToDisplay { get; set; }

        private bool _checked;

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    NotifyPropertyChanged("Checked");
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(
                        new DocSearcher.Message.ClearScaningFilePathMessage());
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}