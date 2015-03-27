using DocSearcher.Control;
using DocSearcher.Message;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using System.Windows;

namespace DocSearcher.View
{
    public partial class ModernMainWindow : ModernWindow
    {
        public ModernMainWindow()
        {
            InitializeComponent();
        }

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MainWindowUidMessage(
                Thread.CurrentThread));
        }
    }
}