﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            labelCurr.Content = "Click Here";
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            labelCurr.Content = "Button clicked";
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            labelCurr.Content = "Button clicked";

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            labelCurr.Content = "Button clicked";

        }
    }
}
