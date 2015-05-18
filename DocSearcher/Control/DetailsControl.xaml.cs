using DocSearcher.Message;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocSearcher.Control
{
    public partial class DetailsControl : UserControl
    {
        private System.Collections.ObjectModel.ObservableCollection<Model.DocTypeCollection> Stats;

        public DetailsControl()
        {
            InitializeComponent();
        }

        internal void GetStats(System.Collections.ObjectModel.ObservableCollection<Model.DocTypeCollection> Stats)
        {
            // passer la liste avc les paths
            this.Stats = Stats;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new ShowChartsMessage());
        }
    }
}