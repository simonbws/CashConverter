using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;

namespace CashConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Root val = new Root();
        public class Root
        {
            public Rate rates { get; set; }
            //public long timestamp;
            //public string license;
        }
        public class Rate
        {
            public double PLN { get; set; }
            public double CZK { get; set; }
            public double USD { get; set; }
            public double HUF { get; set; }
            public double NOK { get; set; }
            public double SEK { get; set; }
            public double EUR { get; set; }
            public double CHF { get; set; }
        }

        //SqlConnection con = new SqlConnection();


        //SqlCommand cmd = new SqlCommand();


        //SqlDataAdapter da = new SqlDataAdapter();

        //private int CurrencyId = 0;
        //private double FromAmount = 0;
        //private double ToAmount = 0;
        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            //BindCurr();
            //BindCurr();
            GetValue();
        }

        private async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=f80bbabe54664ab8a2a359064633ed54");
            BindCurr();
        }

        public static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ResponseString = await response.Content.ReadAsStringAsync();
                        var ResponseObject = JsonConvert.DeserializeObject<Root>(ResponseString);

                        //MessageBox.Show("Rates: " + ResponseString, "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                        return ResponseObject;
                    }
                    return myRoot;
                }

            }
            catch
            {
                return myRoot;
            }
        }
        //public void mycon()
        //{
        //    String Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //    con = new SqlConnection(Conn);
        //    con.Open();
        //}
        private void BindCurr()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Text");

            dt.Columns.Add("Rate");

            dt.Rows.Add("SELECT", 0);
            dt.Rows.Add("PLN", val.rates.PLN);
            dt.Rows.Add("CZK", val.rates.CZK);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("HUF", val.rates.HUF);
            dt.Rows.Add("NOK", val.rates.NOK);
            dt.Rows.Add("SEK", val.rates.SEK);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("CHF", val.rates.CHF);

            cmbFromCurr.ItemsSource = dt.DefaultView;

            cmbFromCurr.DisplayMemberPath = "Text";
            cmbFromCurr.SelectedValuePath = "Rate";
            cmbFromCurr.SelectedIndex = 0;

            cmbToCurr.ItemsSource = dt.DefaultView;

            cmbToCurr.DisplayMemberPath = "Text";
            cmbToCurr.SelectedValuePath = "Rate";
            cmbToCurr.SelectedIndex = 0;



            //mycon();


            //cmd = new SqlCommand("select Id, CurrencyName from Currency_Advanced", con);
            //cmd.CommandType = CommandType.Text;

            //da = new SqlDataAdapter(cmd);
            //da.Fill(dt);

            //DataRow newRow = dt.NewRow();
            //newRow["Id"] = 0;
            //newRow["CurrencyName"] = "SELECT";

            //dt.Rows.InsertAt(newRow, 0);

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    cmbFromCurr.ItemsSource = dt.DefaultView;
            //    cmbToCurr.ItemsSource = dt.DefaultView;
            //}
            //con.Close();

            //DataTable dtCurr = new DataTable();
            //dtCurr.Columns.Add("Text");
            //dtCurr.Columns.Add("Value");


            //Value and Text needs to be added to database
            //dtCurr.Rows.Add("SELECT", 0);
            //dtCurr.Rows.Add("INR", 1);
            //dtCurr.Rows.Add("USD", 70);
            //dtCurr.Rows.Add("EUR", 85);
            //dtCurr.Rows.Add("SAR", 20);
            //dtCurr.Rows.Add("POUND", 5);
            //dtCurr.Rows.Add("ZL", 32);



            //cmbFromCurr.DisplayMemberPath = "CurrencyName";
            //cmbFromCurr.SelectedValuePath = "Id";
            //cmbFromCurr.SelectedIndex = 0;


            //cmbToCurr.DisplayMemberPath = "CurrencyName";
            //cmbToCurr.SelectedValuePath = "Id";
            //cmbToCurr.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double TransformedValue;

            if (textCurr.Text == null || textCurr.Text.Trim() == "")
            {
                MessageBox.Show("Please provide currency you want to transform", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                textCurr.Focus();
                return;
            }
            else if (cmbFromCurr.SelectedValue == null || cmbFromCurr.SelectedIndex == 0 || cmbFromCurr.Text == "SELECT" )
            {
                MessageBox.Show("Please Select Currency From", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurr.Focus();
                return;
            }
            else if (cmbToCurr.SelectedValue == null || cmbToCurr.SelectedIndex == 0 || cmbToCurr.Text == "SELECT")
            {
                //Show the message
                MessageBox.Show("Please Select Currency To", "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus on the To Combobox
                cmbToCurr.Focus();
                return;
            }
            if (cmbFromCurr.Text == cmbToCurr.Text)
            {
                TransformedValue = double.Parse(textCurr.Text);
                labelCurr.Content = cmbToCurr.Text + " " + TransformedValue.ToString("N3");
            }
            else
            {
                TransformedValue = (double.Parse(cmbToCurr.SelectedValue.ToString()) *
                    double.Parse(textCurr.Text)) / double.Parse(cmbFromCurr.SelectedValue.ToString());

                labelCurr.Content = cmbToCurr.Text + " " + TransformedValue.ToString("N3");

            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();

        }

        private void ClearControls()
        {
            textCurr.Text = String.Empty;
            if (cmbFromCurr.Items.Count > 0)
                cmbFromCurr.SelectedIndex = 0;
            if (cmbToCurr.Items.Count > 0)
                cmbToCurr.SelectedIndex = 0;
            labelCurr.Content = "";
            textCurr.Focus();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^0-9]+");
            e.Handled = rgx.IsMatch(e.Text);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        //private void btnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Enter correct amount", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        //            txtAmount.Focus();
        //            return;
        //        }
        //        else if (txtCurrencyName.Text == null || txtCurrencyName.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Enter correct currency name", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        //            txtCurrencyName.Focus();
        //            return;

        //        }
        //        else
        //        {
        //            if (CurrencyId > 0) //check if id is greater than "SELECT" because we have to choose one of the proper currency like euro etc.
        //            {
        //                if (MessageBox.Show("Do you want to update ?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) //if user click yes, this is gonna to be perform, if not, it goes to else instructions 
        //                {
        //                    //mycon();
        //                    DataTable dt = new DataTable();

        //                    //Update Query Record update using Id
        //                    cmd = new SqlCommand("UPDATE Currency_Advanced SET Amount = @Amount, CurrencyName = @CurrencyName WHERE Id = @Id", con);
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Parameters.AddWithValue("@Id", CurrencyId);
        //                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
        //                    cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();

        //                    MessageBox.Show("Data has been uploaded", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        //                }
        //            }
        //            else
        //            {
        //                if (MessageBox.Show("Do you want to save ?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //                {
        //                    //mycon();
        //                    //Insert query to Save data in the table
        //                    cmd = new SqlCommand("INSERT INTO Currency_Advanced(Amount, CurrencyName) VALUES(@Amount, @CurrencyName)", con);
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
        //                    cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();

        //                    MessageBox.Show("Data saved successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        //                }

        //            }
        //            ClearMaster();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //}
        //private void ClearMaster()
        //{
        //    try
        //    {
        //        txtAmount.Text = string.Empty; //we clearing the text fields and below get the data
        //        txtCurrencyName.Text = string.Empty;
        //        buttonSave.Content = "Save";
        //        GetData();
        //        CurrencyId = 0;
        //        BindCurr();
        //        txtAmount.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        //public void GetData()
        //{

        //    //mycon();

        //    DataTable dt = new DataTable();

        //    cmd = new SqlCommand("SELECT * FROM Currency_Advanced", con);

        //    cmd.CommandType = CommandType.Text;

        //    da = new SqlDataAdapter(cmd);

        //    da.Fill(dt);

        //    if (dt != null && dt.Rows.Count > 0)
        //        dgvCurr.ItemsSource = dt.DefaultView;
        //    else
        //        dgvCurr.ItemsSource = null;

        //    con.Close();
        //}

        //private void btnCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ClearMaster();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    try
        //    {
        //        //Create object for DataGrid
        //        DataGrid grd = (DataGrid)sender;

        //        //Create an object for DataRowView
        //        DataRowView row_selected = grd.CurrentItem as DataRowView;

        //        //If row_selected is not null
        //        if (row_selected != null)
        //        {
        //            //dgvCurrency items count greater than zero
        //            if (dgvCurr.Items.Count > 0)
        //            {
        //                if (grd.SelectedCells.Count > 0)
        //                {
        //                    //Get selected row id column value and set it to the CurrencyId variable
        //                    CurrencyId = Int32.Parse(row_selected["Id"].ToString());

        //                    //DisplayIndex is equal to zero in the Edited cell
        //                    if (grd.SelectedCells[0].Column.DisplayIndex == 0)
        //                    {
        //                        //Get selected row amount column value and set to amount textbox
        //                        txtAmount.Text = row_selected["Amount"].ToString();

        //                        //Get selected row CurrencyName column value and set it to CurrencyName textbox
        //                        txtCurrencyName.Text = row_selected["CurrencyName"].ToString();
        //                        buttonSave.Content = "Update";     //Change save button text Save to Update
        //                    }

        //                    //DisplayIndex is equal to one in the deleted cell
        //                    if (grd.SelectedCells[0].Column.DisplayIndex == 1)
        //                    {
        //                        //Show confirmation dialog box
        //                        if (MessageBox.Show("Are you sure you want to delete ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //                        {
        //                            //mycon();
        //                            DataTable dt = new DataTable();

        //                            //Execute delete query to delete record from table using Id
        //                            cmd = new SqlCommand("DELETE FROM Currency_Advanced WHERE Id = @Id", con);
        //                            cmd.CommandType = CommandType.Text;

        //                            //CurrencyId set in @Id parameter and send it in delete statement
        //                            cmd.Parameters.AddWithValue("@Id", CurrencyId);
        //                            cmd.ExecuteNonQuery();
        //                            con.Close();

        //                            MessageBox.Show("Data deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        //                            ClearMaster();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}
