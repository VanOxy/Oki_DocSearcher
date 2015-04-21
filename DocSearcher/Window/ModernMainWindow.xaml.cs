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
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
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
            }
        }

        private void AdaptWindowForGraphicsMode()
        {
            this.Height = SystemParameters.PrimaryScreenHeight * 0.6;
            this.Width = SystemParameters.PrimaryScreenWidth * 0.6;
            CenterWindowOnScreen();
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
        }

        private void AdaptWindowForResearchMode()
        {
            this.Height = 130;
            this.Width = 605;
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
        }

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MainWindowUidMessage(
                Thread.CurrentThread));
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}