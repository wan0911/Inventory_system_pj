﻿using System.Windows;

using static materials_management.MainWindow;
using materials_management.ViewModels;
using static materials_management.Models.DatabaseModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System;

namespace materials_management
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel viewModel = new MainViewModel();
            //DataContext = viewModel
            DataContext = App.Current.Services.GetService((typeof(MainViewModel)));
        }
    }
}
