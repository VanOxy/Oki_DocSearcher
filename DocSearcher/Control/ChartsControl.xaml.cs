using De.TorstenMandelkow.MetroChart;
using DocSearcher.Message;
using DocSearcher.Model;
using GalaSoft.MvvmLight.Messaging;
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
        private bool _isInMB;
        private bool _isColumnChart;

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
            data = collection;
            DataContext = this;
            _isInMB = true;
            _isColumnChart = false;

            InitializeComponent();
        }

        public void InitCharts()
        {
            var myChart = new StackedColumnChart();
            ActiveChart.Content = myChart;
            myChart.ChartTitle = "Total size : ";

            if (_isInMB)
                myChart.ChartSubTitle = "In Megabytes";
            else
                myChart.ChartSubTitle = "In Gigabytes";

            // Clear all current series (including the dummy 1st one the first time)
            myChart.Series.Clear();

            foreach (var item in data)
            {
                ObservableCollection<ExtensionSpace> Series = new ObservableCollection<ExtensionSpace>();

                // Create and configure series
                ChartSeries serie = new ChartSeries();
                serie.SeriesTitle = item.Type;

                foreach (var itm in item.Extensions)
                {
                    if (_isInMB == true)
                        Series.Add(itm);
                    else
                        Series.Add(new FloatExtensionSpace()
                        {
                            Extension = itm.Extension,
                            Space = Convert.ToDouble((itm.Space / 1024.0).ToString("N2"))
                        });
                }

                serie.DisplayMember = "Extension";
                serie.ValueMember = "Space";
                // Important: if you want the graph to update when adding,
                // removing or chaning series, set ItemsSource to null first (this will force it to update)
                serie.ItemsSource = null;
                // Then add to chart and set to actual data source
                myChart.Series.Add(serie);
                serie.ItemsSource = Series;
            }
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var value = ((RadioButton)sender).Content.ToString();
            if (value == "Columns")
            {
                InitColumnChart();
                _isColumnChart = true;
            }
            else
            {
                InitCharts();
                _isColumnChart = false;
            }
        }

        private void RadioButton_Size_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            string size = ((RadioButton)sender).Content.ToString();

            switch (size)
            {
                case "MB":
                    _isInMB = true;
                    if (_isColumnChart)
                        InitColumnChart();
                    else
                        InitCharts();
                    break;

                case "GB":
                    _isInMB = false;
                    if (_isColumnChart)
                        InitColumnChart();
                    else
                        InitCharts();
                    break;
            }
        }

        private void InitColumnChart()
        {
            ActiveChart.Content = null;

            var myChart = new ClusteredColumnChart();
            ActiveChart.Content = myChart;
            myChart.ChartTitle = "Total size : ";

            if (_isInMB)
                myChart.ChartSubTitle = "In Megabytes";
            else
                myChart.ChartSubTitle = "In Gigabytes";

            myChart.Series.Clear();

            foreach (var item in data)
            {
                ObservableCollection<ExtensionSpace> Series = new ObservableCollection<ExtensionSpace>();

                // Create and configure series
                ChartSeries serie = new ChartSeries();
                serie.SeriesTitle = item.Type;

                foreach (var itm in item.Extensions)
                {
                    if (_isInMB == true)
                        Series.Add(itm);
                    else
                        Series.Add(new FloatExtensionSpace()
                        {
                            Extension = itm.Extension,
                            Space = Convert.ToDouble((itm.Space / 1024.0).ToString("N2"))
                        });
                }

                serie.DisplayMember = "Extension";
                serie.ValueMember = "Space";
                serie.ItemsSource = null;
                myChart.Series.Add(serie);
                serie.ItemsSource = Series;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void MoreInformations_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Messenger.Default.Send(new ShowDetailsMessage());
        }

        private void NewResearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Messenger.Default.Send(new ShowSelectionMessage());
        }
    }
}