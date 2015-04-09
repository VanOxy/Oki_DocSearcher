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

            Messenger.Default.Register<ChangeWindowSizeMessage>(this, ChangeWindowSize);
        }

        private void ChangeWindowSize(ChangeWindowSizeMessage message)
        {
            switch (message.Stage)
            {
                case "research":
                    AdaptWindowForResearchMode();
                    break;

                case "stat":
                    AdaptWindowForGraphicsMode();
                    break;

                default:
                    break;
            }
        }

        private void AdaptWindowForGraphicsMode()
        {
            // adaptive size mode...
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
        }

        private void AdaptWindowForResearchMode()
        {
            this.Height = 130;
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
        }

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MainWindowUidMessage(
                Thread.CurrentThread));
        }
    }
}