using DocSearcher.Message;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace DocSearcher.Control
{
    public partial class ExtensionsManagementControl : UserControl
    {
        public ExtensionsManagementControl()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Messenger.Default.Send(new ExtensionManagementFileTypeSelectedMessage((sender as ComboBox).SelectedValue.ToString()));
        }
    }
}