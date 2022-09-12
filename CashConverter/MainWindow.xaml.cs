using System;
using System.Collections.Generic;
using System.Data;
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
            BindCurr();
        }
        private void BindCurr()
        {
            DataTable dtCurr = new DataTable();
            dtCurr.Columns.Add("Text");
            dtCurr.Columns.Add("Value");
            

            //Value and Text needs to be added to database
            dtCurr.Rows.Add("SELECT", 0);
            dtCurr.Rows.Add("INR", 1);
            dtCurr.Rows.Add("USD", 75);
            dtCurr.Rows.Add("EUR", 85);
            dtCurr.Rows.Add("SAR", 20);
            dtCurr.Rows.Add("POUND", 5);
            dtCurr.Rows.Add("ZL", 0.75);

            //We assign data currency to items source of CmbFromCurrency
            cmbFromCurr.ItemsSource = dtCurr.DefaultView;
            cmbFromCurr.DisplayMemberPath = "Text";
            cmbFromCurr.SelectedValuePath = "Value";
            cmbFromCurr.SelectedIndex = 0;

            cmbToCurr.ItemsSource = dtCurr.DefaultView;
            cmbToCurr.DisplayMemberPath= "Text";
            cmbToCurr.SelectedValuePath="Value";
            cmbToCurr.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double TransformedValue;

            if (textCurr.Text == null || textCurr.Text.Length == 0)
            {
                MessageBox.Show("Please provide currency you want to transform", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                textCurr.Focus();
                return;
            }
            else if (cmbFromCurr.SelectedValue == null || cmbFromCurr.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency From", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurr.Focus();
                return;
            }

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
