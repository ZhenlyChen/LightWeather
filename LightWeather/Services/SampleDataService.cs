using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using LightWeather.Models;

using Windows.UI.Xaml.Controls;

namespace LightWeather.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Delete this file once your app is using real data.
    public static class SampleDataService
    {
        private static IEnumerable<SampleOrder> AllOrders()
        {
            // The following is order summary data
            var data = new ObservableCollection<SampleOrder>
            {
                new SampleOrder
                {
                    OrderId = 70,
                    OrderDate = new DateTime(2017, 05, 24),
                    Company = "Company F",
                    ShipTo = "Francisco Pérez-Olaeta",
                    OrderTotal = 2490.00,
                    Status = "Closed",
                    Symbol = Symbol.Globe
                },
            };

            return data;
        }

        // TODO WTS: Remove this once your chart page is displaying real data
        public static ObservableCollection<DataPoint> GetChartSampleData()
        {
            var data = AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
    }
}
