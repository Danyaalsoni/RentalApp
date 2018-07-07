using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace RentalApp
{
    /// <summary>
    /// Interaction logic for RenterDetailedView.xaml
    /// </summary>
    public partial class RenterDetailedView : Window
    {
        int renterID, addressID;
        string dbloc;
        List<Renter> renterList = new List<Renter>();
        public RenterDetailedView(int addressID, int renterID)
        {
            InitializeComponent();
            
            this.renterID = renterID;
            this.addressID = addressID;
            dbloc = ((MainWindow)Application.Current.MainWindow).dbloc;
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }
            loadItems();
        }
        private void loadItems()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {

                conn.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("SELECT * from Renter WHERE AddressID=" + addressID + " AND ID=" + renterID, conn);
                    SQLiteDataReader reader1 = command.ExecuteReader();

                    while (reader1.Read())
                    {
                        nameText.Text = reader1["TenantName"].ToString();

                        phoneText.Text = reader1["Phone"].ToString();
                        emailText.Text = reader1["Email"].ToString();
                        RentText.Text = reader1["Rent"].ToString();
                        startDateText.Text = reader1["StartDate"].ToString();
                        endDateText.Text = reader1["EndDate"].ToString();
                        depositText.Text = reader1["Deposit"].ToString();
                        depositDateText.Text = reader1["DepositDate"].ToString();
                        cleaningDepositText.Text = reader1["CleaningDeposit"].ToString();
                        cleaningDepositDateText.Text = reader1["CleaningDepositDate"].ToString();
                        keyDepositText.Text = reader1["KeyDepositDate"].ToString();
                        keyDepositDateText.Text = reader1["KeyDeposit"].ToString();
                        string ren30 = reader1["Renewal_in_30"].ToString();
                        string ren90 = reader1["Renewal_in_90"].ToString();
                        rentDateText.Text = reader1["RentDate"].ToString();

                        if (ren30 == "YES" && ren90 == "NO")
                        {

                            renewalText.Text = "Renewal in 30";
                        }
                        else if (ren90 == "YES" && ren30 == "NO")
                        {

                            renewalText.Text = "Renewal in 90";
                        }
                        else
                        {

                            renewalText.Text = "No Renewal";
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error in select all rental address statement");
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //done
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //edit
            EditRenter editRenter = new EditRenter(renterID, addressID);
            editRenter.ShowDialog();
            loadItems();
        }
    }
}
