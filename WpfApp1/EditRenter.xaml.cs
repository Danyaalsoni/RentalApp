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
using System.Data;
namespace RentalApp
{
    /// <summary>
    /// Interaction logic for EditRenter.xaml
    /// </summary>
    
    public partial class EditRenter : Window
    {
        string dbloc;
        int renterID, addressID;
        List<Renter> renterList = new List<Renter>();
        public EditRenter(int renterID,int addressID)
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
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {

                conn.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("SELECT * from Renter WHERE AddressID=" + addressID +" AND ID="+renterID, conn);
                    SQLiteDataReader reader1 = command.ExecuteReader();

                    while (reader1.Read())
                    {
                        addNameBox.Text = reader1["TenantName"].ToString();

                        addPhoneNumberBox.Text = reader1["Phone"].ToString();
                        addEmailBox.Text = reader1["Email"].ToString();
                        addRentBox.Text= reader1["Rent"].ToString();
                        addStartDateBox.Text = reader1["StartDate"].ToString();
                        addEndDateBox.Text = reader1["EndDate"].ToString();
                        addDepositBox.Text= reader1["Deposit"].ToString();
                        addDepositDateBox.Text= reader1["DepositDate"].ToString();
                        addCleaningDepositBox.Text= reader1["CleaningDeposit"].ToString();
                        addCleaningDateBox.Text= reader1["CleaningDepositDate"].ToString();
                        addKeyDateBox.Text = reader1["KeyDepositDate"].ToString();
                        addKeyDepositBox.Text= reader1["KeyDeposit"].ToString();
                        string ren30 = reader1["Renewal_in_30"].ToString();
                        string ren90 = reader1["Renewal_in_90"].ToString();
                       
                        if (ren30 == "YES" && ren90 == "NO")
                        {
                            
                            addRenewalCombo.SelectedIndex = 0;
                        }
                        else if (ren90 == "YES" && ren30 == "NO")
                        {
                            
                            addRenewalCombo.SelectedIndex = 1;
                        }
                        else
                        {
                           
                            addRenewalCombo.SelectedIndex = 2;
                        }
                       
                        
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error in select all rental address statement");
                }

            }
            

        }
        public void Save_Renter_Button_Click(object sender, RoutedEventArgs e)
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
                }

            }
            save();
        }
        private void save()
        {
            if (renterList.Count != 0)
            {

                using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                {
                    conn.Open();
                    foreach (Renter renter in renterList)
                    {

                        //SQLiteCommand command1 = new SQLiteCommand("UPDATE RentalAddress SET Address=:address WHERE ID=:id", conn);
                        //command1.Parameters.Add("address", DbType.String).Value = address;
                        //command1.Parameters.Add("id", DbType.Int32).Value = addressID;

                        SQLiteCommand command1 = new SQLiteCommand("UPDATE Renter SET TenantName=:name,Phone=:phone,Email=:email,Rent=:rent,StartDate=:startDate,EndDate=:endDate,Deposit=:deposit,CleaningDeposit=:cleaningDeposit,KeyDeposit=:keyDeposit,Renewal_in_30=:ren30,Renewal_in_90=:ren90,CleaningDepositDate=:cleaningDepDate,KeyDepositDate=:keyDepDate,DepositDate=:depDate WHERE ID=:id AND AddressID=:addressid", conn);
                        command1.Parameters.Add("name",DbType.String).Value= renter.TenantName;
                        command1.Parameters.Add("phone",DbType.String).Value=renter.Phone;
                        command1.Parameters.Add("email",DbType.String).Value=renter.Email;
                        command1.Parameters.Add("rent",DbType.VarNumeric).Value=renter.Rent;
                        command1.Parameters.Add("startDate",DbType.String).Value=renter.StartDate;
                        command1.Parameters.Add("endDate",DbType.String).Value=renter.EndDate;
                        command1.Parameters.Add("deposit",DbType.VarNumeric).Value=renter.deposit;
                        command1.Parameters.Add("cleaningDeposit",DbType.VarNumeric).Value=renter.cleaningDeposit;
                        command1.Parameters.Add("keyDeposit",DbType.VarNumeric).Value=renter.keyDeposit;
                        command1.Parameters.Add("ren30",DbType.String).Value= renter.Renewal_in_30;
                        command1.Parameters.Add("ren90",DbType.String).Value= renter.Renewal_in_90;
                        command1.Parameters.Add("cleaningDepDate",DbType.String).Value= renter.cleaningDepositDate;
                        command1.Parameters.Add("keyDepDate",DbType.String).Value= renter.keyDepositDate;
                        command1.Parameters.Add("depDate", DbType.String).Value= renter.depositDate;
                        command1.Parameters.Add("id", DbType.Int32).Value = renterID;
                        command1.Parameters.Add("addressid", DbType.Int32).Value = addressID;
                        command1.ExecuteNonQuery();
                    }

                }
            }

            MessageBox.Show("Saved successfully");
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
