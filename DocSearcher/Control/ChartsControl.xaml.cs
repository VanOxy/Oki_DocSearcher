using De.TorstenMandelkow.MetroChart;
using DocSearcher.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DocSearcher.Control
{
    public partial class ChartsControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<DocTypeCollection> data;

        public ObservableCollection<string> DataRepresentationList { get; set; }

        private UserControl _activeChart = new UserControl();

        public UserControl ActiveChart
        {
            get
            {
                return _activeChart;
            }
            set
            {
                if (_activeChart != value)
                {
                    _activeChart = value;
                    NotifyPropertyChanged("ActiveChart");
                }
            }
        }

        public ChartsControl(ObservableCollection<DocTypeCollection> collection)
        {
            InitializeComponent();
            data = collection;
            DataContext = this;

            //InitAndFillDataRepresentationList();
        }

        //<chart:StackedColumnChart x:Name="MyChart" ChartTitle="Stats :" ChartSubTitle="In Megabytes">
        //    <chart:StackedColumnChart.Series>
        //        <chart:ChartSeries />
        //    </chart:StackedColumnChart.Series>
        //</chart:StackedColumnChart>

        public void InitCharts()
        {
            var myChart = new StackedColumnChart();

            //var userControl = new UserControl();
            //userControl.Content = chart;
            //ActiveChart = userControl;

            ActiveChart.Content = myChart;

            // Clear all current series (including the dummy 1st one the first time)
            myChart.Series.Clear();

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
                myChart.Series.Add(serie);
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