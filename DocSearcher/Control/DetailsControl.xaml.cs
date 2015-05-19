using DocSearcher.Message;
using DocSearcher.Model;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            ExtensionsList.Items.Clear();

            var necessaryObject = (from obj in _stats
                                   where obj.Type == selectedCategorie
                                   select obj.Extensions).FirstOrDefault();
            var extensionsContext = (from a in necessaryObject
                                     select a.Extension).ToList();

            foreach (var item in extensionsContext)
            {
                ExtensionsList.Items.Add(item.ToString());
            }
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
            ExtensionsList.Items.Clear();
            ExtensionsChoiseZone.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}