using DocSearcher.Message;
using DocSearcher.Model;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private List<string> FillExtensionsList(string selectedCategorie)
        {
            ExtensionsList.Items.Clear();

            var tempList = new List<string>();

            var necessaryObject = (from obj in _stats
                                   where obj.Type == selectedCategorie
                                   select obj.Extensions).FirstOrDefault();
            var extensionsContext = (from a in necessaryObject
                                     select a.Extension).ToList();

            foreach (var item in extensionsContext)
            {
                ExtensionsList.Items.Add(item.ToString());
                tempList.Add(item.ToString());
            }
            return tempList;
        }

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).Items.Count > 0)
            {
                string selectedCategorie = ((ComboBox)sender).SelectedItem.ToString();

                ExtensionsChoiseZone.Visibility = System.Windows.Visibility.Visible;

                List<string> extList = FillExtensionsList(selectedCategorie);
                FillPathsList(extList);
            }
        }

        private void ExtensionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).Items.Count > 0)
            {
                List<string> temp = new List<string>();
                temp.Add(((ComboBox)sender).SelectedItem.ToString());
                FillPathsList(temp);
            }
        }

        private void FillPathsList(List<string> extList)
        {
            // add point at begin of extensions to avoid string casting the whole list of paths
            List<string> localExtensionsList = new List<string>();
            foreach (var item in extList)
                localExtensionsList.Add("." + item);

            PathsList.Items.Clear();
            foreach (var str in _paths)
                if (localExtensionsList.Contains(Path.GetExtension(str)))
                    PathsList.Items.Add(str);
        }

        internal void ClearValues()
        {
            CategoriesList.Items.Clear();
            ExtensionsList.Items.Clear();
            ExtensionsChoiseZone.Visibility = System.Windows.Visibility.Hidden;
        }

        private void PathsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string filePath = ((ListBox)sender).SelectedItem.ToString();
            string argument = @"/select, " + filePath;

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
    }
}