using RentalApp.Database;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for RenterInfo.xaml
    /// </summary>
    public partial class RenterInfo : Window
    {
        List<Renter> renterList = new List<Renter>();
        string dbloc;
        int addressID;
        public RenterInfo(int addressID)
        {
            this.addressID = addressID;
            InitializeComponent();
            dbloc = ((MainWindow)Application.Current.MainWindow).dbloc;
            renterListView.ClipToBounds = true;
            renterListView.SelectionMode = SelectionMode.Single;
            addstreetAddressBox.IsEnabled = false;
            // renterListView.IsHitTestVisible = false;
            loadItems();
            
        }
        private void loadItems()
        {
            renterList.Clear();
            this.renterListView.Items.Clear();
            renterListView.ClipToBounds = true;
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {
                conn.Open();
                // SQLiteCommand command = new SQLiteCommand("INSERT into RentalAddress (ID,Address,RenterID) Values (4,'ss',0)", conn);
                //command.ExecuteNonQuery();
                SQLiteCommand command1 = new SQLiteCommand("Select * from RentalAddress WHERE ID= " + this.addressID, conn);
                SQLiteDataReader reader = command1.ExecuteReader();

                while (reader.Read())
                {
                    //Console.WriteLine(reader["YourColumn"]);
                    colorZoneText.Text = "Renters renting From:  " + reader["Address"].ToString();
                    //renterList.Add(new Renter(Convert.ToInt32(reader["ID"]), reader["TenantName"].ToString(), reader["Phone"].ToString(), reader["Email"].ToString(), double.Parse(reader["Rent"].ToString()), reader["StartDate"].ToString(), reader["EndDate"].ToString(), double.Parse(reader["Deposit"].ToString()), double.Parse(reader["CleaningDeposit"].ToString()), double.Parse(reader["KeyDeposit"].ToString()), reader["Renewal_in_30"].ToString(), reader["Renewal_in_90"].ToString(), reader["DepositDate"].ToString(), reader["KeyDepositDate"].ToString(), reader["CleaningDepositDate"].ToString()));

                }
                reader.Close();

            }
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {
                conn.Open();
                // SQLiteCommand command = new SQLiteCommand("INSERT into RentalAddress (ID,Address,RenterID) Values (4,'ss',0)", conn);
                //command.ExecuteNonQuery();
                SQLiteCommand command2 = new SQLiteCommand("Select * from Renter WHERE AddressID= " + this.addressID, conn);
                SQLiteDataReader reader = command2.ExecuteReader();

                while (reader.Read())
                {
                    //Console.WriteLine(reader["YourColumn"]);

                    renterList.Add(new Renter(Convert.ToInt32(reader["ID"]), reader["TenantName"].ToString(), reader["Phone"].ToString(), reader["Email"].ToString(), double.Parse(reader["Rent"].ToString()), reader["StartDate"].ToString(), reader["EndDate"].ToString(), double.Parse(reader["Deposit"].ToString()), double.Parse(reader["CleaningDeposit"].ToString()), double.Parse(reader["KeyDeposit"].ToString()), reader["Renewal_in_30"].ToString(), reader["Renewal_in_90"].ToString(), reader["DepositDate"].ToString(), reader["KeyDepositDate"].ToString(), reader["CleaningDepositDate"].ToString(), reader["RentDate"].ToString()));

                }
                reader.Close();
                foreach (Renter r in renterList)
                {
                    this.renterListView.Items.Add(r);
                }
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
            if (renterListView.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a renter from the List view");

            }
            else
            {
                var row = renterListView.SelectedItem as Renter;
                EditRenter editRenter = new EditRenter(row.ID, addressID);
                editRenter.ShowDialog();
                loadItems();
            }

        }
        bool editFlag = true;
        private void Edit_Address_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag)
            {
                addstreetAddressBox.IsEnabled = true;
                editAddressButton.Content = "Save Address";
                editFlag = false;
            }
            else
            {
                string address= addstreetAddressBox.Text;
                if (address == "")
                {
                    MessageBox.Show("Please enter address");

                }
                else
                {
                    using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                    {
                        conn.Open();
                        SQLiteCommand command1 = new SQLiteCommand("UPDATE RentalAddress SET Address=:address WHERE ID=:id", conn);
                        command1.Parameters.Add("address", DbType.String).Value = address;
                        command1.Parameters.Add("id", DbType.Int32).Value = addressID;
                        command1.ExecuteNonQuery();
                    }
                    addstreetAddressBox.IsEnabled = false;
                    editAddressButton.Content = "Edit Address";
                    editFlag = true;
                    MessageBox.Show("Updated successfully");
                }
            }
        }
        private void Add_Renter_Click(object sender, RoutedEventArgs e)
        {
            Add_Renter add_Renter = new Add_Renter(addressID);
            add_Renter.ShowDialog();
            loadItems();
        }
        private void done_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Renter rowID;
            int id = -1;
            rowID = renterListView.SelectedItem as Renter;
            if (rowID != null)
            {
                id = rowID.ID;
            }
            RenterDetailedView renterDetailedView = new RenterDetailedView(this.addressID,id);
            renterDetailedView.ShowDialog();
            //}
            loadItems();
        }
    }

}
