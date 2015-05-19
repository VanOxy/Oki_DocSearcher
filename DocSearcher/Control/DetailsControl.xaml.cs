using DocSearcher.Message;
using DocSearcher.Model;
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
        private List<string> _paths;
        private System.Collections.ObjectModel.ObservableCollection<DocTypeCollection> _stats;
        private bool _activated = false;

        public bool Activated
        {
            get { return _activated; }
            set { _activated = value; }
        }

        public DetailsControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new ShowChartsMessage());
        }

        internal void GetStats(List<string> paths,
            System.Collections.ObjectModel.ObservableCollection<DocTypeCollection> Stats)
        {
            _stats = Stats;
            _paths = paths;
        }

        #region Tools

        public void InitValues()
        {
            // make activated for further use
            // to avoid variable reassignation
            Activated = true;

            FillCategoriesList();
        }

        private void FillCategoriesList()
        {
            foreach (var item in _stats)
            {
                CategoriesList.Items.Add(item.Type.ToString());
            }
        }

        private void FillExtensionsList(string selectedCategorie)
        {
            // todo
        }

        #endregion Tools

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).Items.Count > 0)
            {
                string selectedCategorie = ((ComboBox)sender).SelectedItem.ToString();

                ExtensionsChoiseZone.Visibility = System.Windows.Visibility.Visible;

                FillExtensionsList(selectedCategorie);
            }
        }

        internal void ClearValues()
        {
            CategoriesList.Items.Clear();
            ExtensionsChoiseZone.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}