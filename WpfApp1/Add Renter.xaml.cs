using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Add_Renter.xaml
    /// </summary>
    public partial class Add_Renter : Window
    {
        List<Renter> renterList = new List<Renter>();
        string dbloc;
        int addressID;
        public Add_Renter(int id)
        {
            InitializeComponent();
            this.addressID = id;
            dbloc = ((MainWindow)Application.Current.MainWindow).dbloc;
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }
        }
        
        private void Add_Renter_Button_Click(object sender, RoutedEventArgs e)
        {
            string tenantName, phoneNumber, emailAddress, renewal, renewalin30 = "NO", renewalin90 = "NO", depositDate = "", cleaningDepositDate = "", keyDepositDate = "";
            int rent = 0, deposit = 0, cleaningDeposit = 0, keyDeposit = 0;
            string startDate, endDate;
            tenantName = addNameBox.Text;
            phoneNumber = addPhoneNumberBox.Text;
            emailAddress = addEmailBox.Text;
            renewal = addRenewalCombo.SelectedValue.ToString();
            startDate = addStartDateBox.Text;
            endDate = addEndDateBox.Text;
            depositDate = addDepositDateBox.Text;
            cleaningDepositDate = addCleaningDateBox.Text;
            keyDepositDate = addKeyDateBox.Text;

            bool flag = true, tenantExists = false;
            Renter renter;
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }
            if (tenantName == "")
            {
                flag = false;
                MessageBox.Show("Please enter Tenant Name");
            }
            else if (cleaningDepositDate == "")
            {
                flag = false;
                MessageBox.Show("Please enter Cleaning Deposit Date");
            }
            else if (keyDepositDate == "")
            {
                flag = false;
                MessageBox.Show("Please enter Key Deposit Date");
            }
            else if (depositDate == "")
            {
                flag = false;
                MessageBox.Show("Please enter Deposit Date");
            }
            else if (renewal == "")
            {
                flag = false;
                MessageBox.Show("Please select if there is a renewal");
            }
            else if (addRentBox.Text == "")
            {
                flag = false;
                MessageBox.Show("Please enter Rent");
            }
            else if (addDepositBox.Text == "")
            {
                flag = false;
                MessageBox.Show("Please enter Deposit");
            }
            else if (addCleaningDepositBox.Text == "")
            {
                flag = false;
                MessageBox.Show("Please enter cleaning deposit");
            }
            else if (addKeyDepositBox.Text == "")
            {
                flag = false;
                MessageBox.Show("Please enter Key Deposit");
            }
            else if (startDate == "")
            {
                flag = false;
                MessageBox.Show("Please enter start date");
            }
            else if (endDate == "")
            {
                flag = false;
                MessageBox.Show("Please enter End Date");
            }
            else if (cleaningDepositDate == "" || depositDate == "" || keyDepositDate == "" || tenantName == "" || renewal == "" || addRentBox.Text == "" || addCleaningDepositBox.Text == "" || addKeyDepositBox.Text == "" || startDate == "" || endDate == "" || phoneNumber == "" || emailAddress == "")
            {
                flag = false;
                MessageBox.Show("Please Enter Missing Values");
            }
            else
            {
                flag = true;
                rent = Convert.ToInt32(addRentBox.Text);
                deposit = Convert.ToInt32(addDepositBox.Text);
                cleaningDeposit = Convert.ToInt32(addCleaningDepositBox.Text);
                keyDeposit = Convert.ToInt32(addKeyDepositBox.Text);
            }
            if (renewal == "Renewal in 30")
            {
                renewalin30 = "YES";
                renewalin90 = "NO";
            }
            else if (renewal == "Renewal in 90")
            {
                renewalin90 = "YES";
                renewalin30 = "NO";
            }
            else
            {
                renewalin30 = "NO";
                renewalin90 = "NO";
            }
            if (flag)
            {


                using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                {
                    conn.Open();
                    SQLiteCommand command1 = new SQLiteCommand("Select * from Renter where TenantName ='" + tenantName + "'" + "AND AddressID=" + addressID, conn);
                    SQLiteDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        tenantExists = true;
                        MessageBox.Show("Renter already exists for this address");
                    }

                }
                foreach (Renter r in renterList)
                {
                    if (r.TenantName == tenantName)
                    {
                        tenantExists = true;
                        MessageBox.Show("Renter already exists for this address");

                    }
                }
                if (!tenantExists)
                {
                    renter = new Renter(tenantName, phoneNumber, emailAddress, rent, startDate, endDate, deposit, cleaningDeposit, keyDeposit, renewalin30, renewalin90, depositDate, keyDepositDate, cleaningDepositDate);

                    renterList.Add(renter);
                    this.renterListView.Items.Add(renter);
                }

            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (renterList.Count != 0)
            {

                using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                {
                    conn.Open();
                    foreach (Renter renter in renterList)
                    {
                        SQLiteCommand command1 = new SQLiteCommand("INSERT INTO Renter (TenantName,Phone,Email,Rent,StartDate,EndDate,Deposit,CleaningDeposit,KeyDeposit,Renewal_in_30,Renewal_in_90,AddressID,CleaningDepositDate,KeyDepositDate,DepositDate) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14,@param15)", conn);
                        command1.Parameters.Add(new SQLiteParameter("@param1", renter.TenantName));
                        command1.Parameters.Add(new SQLiteParameter("@param2", renter.Phone));
                        command1.Parameters.Add(new SQLiteParameter("@param3", renter.Email));
                        command1.Parameters.Add(new SQLiteParameter("@param4", renter.Rent));
                        command1.Parameters.Add(new SQLiteParameter("@param5", renter.StartDate));
                        command1.Parameters.Add(new SQLiteParameter("@param6", renter.EndDate));
                        command1.Parameters.Add(new SQLiteParameter("@param7", renter.deposit));
                        command1.Parameters.Add(new SQLiteParameter("@param8", renter.cleaningDeposit));
                        command1.Parameters.Add(new SQLiteParameter("@param9", renter.keyDeposit));
                        command1.Parameters.Add(new SQLiteParameter("@param10", renter.Renewal_in_30));
                        command1.Parameters.Add(new SQLiteParameter("@param11", renter.Renewal_in_90));
                        command1.Parameters.Add(new SQLiteParameter("@param12", addressID));
                        command1.Parameters.Add(new SQLiteParameter("@param13", renter.cleaningDepositDate));
                        command1.Parameters.Add(new SQLiteParameter("@param14", renter.keyDepositDate));
                        command1.Parameters.Add(new SQLiteParameter("@param15", renter.depositDate));
                        command1.ExecuteNonQuery();
                    }

                }
            }

            MessageBox.Show("Saved successfully");
            this.Close();
        }
    }
}
