﻿using System;

using LightWeather.ViewModels;

using Windows.UI.Xaml.Controls;

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
        }
    }
}
