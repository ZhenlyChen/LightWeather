using System;
using System.Text;
using LightWeather.ViewModels;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;
using LightWeather.Models;

namespace LightWeather.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
            // ChartFrame.Navigate(typeof(ChartPage));
        }

        private async void GetWeatherByJson_Click(object sender, RoutedEventArgs e) {
            JsonObject data = await Services.WeatherService.GetJson(LocalTextBox.Text);
            StringBuilder str = new StringBuilder();
            if (data["message"].GetString().Equals("Success !")) {
                JsonObject info = data["data"].GetObject();
                str.AppendFormat("城市：{0}\n", LocalTextBox.Text);
                str.AppendFormat("当前温度：{0}\n", info["wendu"].GetString());
                str.AppendFormat("当前湿度：{0}\n", info["shidu"].GetString());
                str.AppendFormat("空气质量：{0}\n", info["quality"].GetString());
                str.AppendFormat("PM25：{0}\n", info["pm25"].GetNumber());
                str.AppendFormat("建议：{0}\n", info["ganmao"].GetString());

                JsonArray forecast = info["forecast"].GetArray();
                var source = new ObservableCollection<ListItem>();
                for (int i = 0; i < forecast.Count; i++) {
                    JsonObject item = forecast[i].GetObject();
                    source.Add(new ListItem {
                        date = item["date"].GetString(),
                        high = item["high"].GetString().Substring(3).Replace(".0", ""),
                        low = item["low"].GetString().Substring(3).Replace(".0", ""),
                        type = item["type"].GetString(),
                        wind = item["fx"].GetString(),
                    });
                }
                ListViewModel.Source = source;
                ChartFrame.Navigate(typeof(ListPage));
            } else {
                str.AppendFormat("未找到城市：{0}\n", LocalTextBox.Text);
            }
            WeatherContent.Text = str.ToString();
        }

        private async void GetWeatherByXML_Click(object sender, RoutedEventArgs e) {
            XmlDocument doc = await Services.WeatherService.GetXml(LocalTextBox.Text);
            StringBuilder str = new StringBuilder();
            XmlElement root = doc.DocumentElement;
            if (root.GetElementsByTagName("error").Length == 0) {
                str.AppendFormat("城市：{0}\n", LocalTextBox.Text);
                str.AppendFormat("当前温度：{0}\n", GetTextFromXml(root, "wendu"));
                str.AppendFormat("当前湿度：{0}\n", GetTextFromXml(root, "shidu"));
                str.AppendFormat("空气质量：{0}\n", GetTextFromXml(root, "quality"));
                str.AppendFormat("PM25：{0}\n", GetTextFromXml(root, "pm25"));
                str.AppendFormat("建议：{0}\n", GetTextFromXml(root, "suggest"));

                XmlNodeList forecast = root.GetElementsByTagName("weather");
                var source = new ObservableCollection<ListItem>();
                for (int i = 0; i < forecast.Count; i++) {
                    XmlElement day = (XmlElement)((XmlElement)forecast[i]).GetElementsByTagName("day")[0];
                    source.Add(new ListItem {
                        date = ((XmlElement)forecast[i]).GetElementsByTagName("date")[0].InnerText,
                        high = ((XmlElement)forecast[i]).GetElementsByTagName("high")[0].InnerText.Substring(3),
                        low =  ((XmlElement)forecast[i]).GetElementsByTagName("low")[0].InnerText.Substring(3),
                        type = day.GetElementsByTagName("type")[0].InnerText,
                        wind = day.GetElementsByTagName("fengxiang")[0].InnerText,
                    });
                }
                ListViewModel.Source = source;
                ChartFrame.Navigate(typeof(ListPage));



            } else {
                str.AppendFormat("未找到城市：{0}\n", LocalTextBox.Text);
            }
            WeatherContent.Text = str.ToString();
        }

        private string GetTextFromXml(XmlElement root, string name) {
            if (root.GetElementsByTagName(name).Length != 0) {
                return root.GetElementsByTagName(name)[0].InnerText;
            } else {
                return "无";
            }
        }
    }
}
