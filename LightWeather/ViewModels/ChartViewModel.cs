using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using LightWeather.Models;
using LightWeather.Services;

namespace LightWeather.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public ChartViewModel()
        {
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetChartSampleData();
            }
        }
    }
}
