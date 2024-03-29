﻿using Gort.UI.View.Pages;
using Gort.UI.ViewModel;
using Gort.UI.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gort.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisposable unsubscriber;
        public MainWindow()
        {
            InitializeComponent();
            ViewModel.Services.MrNav = _mainFrame.NavigationService;
        }
    }
}
