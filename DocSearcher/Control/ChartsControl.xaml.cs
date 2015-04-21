using De.TorstenMandelkow.MetroChart;
using DocSearcher.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DocSearcher.Control
{
    public partial class ChartsControl : UserControl
    {
        private ObservableCollection<DocTypeCollection> data;

        //private List<string> _dataRepresentationList;
        //public List<string> DataRepresentationList
        //{
        //    get
        //    {
        //        return _dataRepresentationList;
        //    }
        //    set
        //    {
        //        if (_dataRepresentationList != value)
        //        {
        //            _dataRepresentationList = value;
        //            NotifyPropertyChanged("DataRepresentationList");
        //        }
        //    }
        //}

        public ChartsControl(ObservableCollection<DocTypeCollection> collection)
        {
            InitializeComponent();
            data = collection;
            DataContext = this;

            //InitAndFillDataRepresentationList();
        }

        public void InitCharts()
        {
            // Clear all current series (including the dummy 1st one the first time)
            MyChart.Series.Clear();

            foreach (var item in data)
            {
                ObservableCollection<ExtensionSpace> Series = new ObservableCollection<ExtensionSpace>();

                // Create and configure series
                ChartSeries serie = new ChartSeries();
                serie.SeriesTitle = item.Type;

                foreach (var itm in item.Extensions)
                    Series.Add(itm);

                serie.DisplayMember = "Extension";
                serie.ValueMember = "Space";
                // Important: if you want the graph to update when adding,
                //removing or chaning series, set ItemsSource to null first (this will force it to update)
                serie.ItemsSource = null;
                // Then add to chart and set to actual data source
                MyChart.Series.Add(serie);
                serie.ItemsSource = Series;
            }
        }

        //#region working...

        //private void InitAndFillDataRepresentationList()
        //{
        //    _dataRepresentationList = new List<string>();
        //    DataRepresentationList.Add("B");
        //    DataRepresentationList.Add("MB");
        //    DataRepresentationList.Add("GB");
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string size = ((ComboBox)sender).SelectedValue.ToString();

        //    switch (size)
        //    {
        //        case "B":
        //            break;

        //        case "MB":
        //            break;

        //        case "GB":
        //            break;
        //    }
        //}

        //private void ModifyDataSize()
        //{
        //    MyChart.Series.Clear();

        //    foreach (var item in data)
        //    {
        //        ObservableCollection<ExtensionSpace> Series = new ObservableCollection<ExtensionSpace>();

        //        // Create and configure series
        //        ChartSeries serie = new ChartSeries();
        //        serie.SeriesTitle = item.Type;

        //        foreach (var itm in item.Extensions)
        //        {
        //            Series.Add(itm);
        //        }

        //        serie.DisplayMember = "Extension";
        //        serie.ValueMember = "Space";
        //        serie.ItemsSource = null;
        //        MyChart.Series.Add(serie);
        //        serie.ItemsSource = Series;
        //    }
        //}

        //#endregion working...
    }
}