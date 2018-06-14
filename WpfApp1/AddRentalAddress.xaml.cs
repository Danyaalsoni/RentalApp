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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddRentalAddress.xaml
    /// </summary>
    public partial class AddRentalAddress : Window
    {
        List<Renter> renterList = new List<Renter>();
        public AddRentalAddress()
        {
            InitializeComponent();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            string address = addstreetAddressBox.Text;
            int count = 0;
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            if (address == "")
            {
                MessageBox.Show("Please Enter Street Address");
                return;
            }
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=|DataDirectory|\DataFile\RentalDatabase.db"))
                {
                    
                    conn.Open();
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand("SELECT * from RentalAddress ", conn);
                        SQLiteDataReader reader1 = command.ExecuteReader();

                        while (reader1.Read())
                        {
                            count++;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error in select all rental address statement");
                    }

                }
                if (renterList.Count != 0)
                {
                    
                    using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=|DataDirectory|\DataFile\RentalDatabase.db"))
                    {
                        conn.Open();
                        foreach (Renter renter in renterList)
                        {
                            SQLiteCommand command1 = new SQLiteCommand("INSERT INTO Renter (TenantName,Phone,Email,Rent,StartDate,EndDate,Deposit,CleaningDeposit,KeyDeposit,Renewal_in_30,Renewal_in_90,AddressID) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12)", conn);
                            command1.Parameters.Add(new SQLiteParameter( "@param1",renter.TenantName));
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
                            command1.Parameters.Add(new SQLiteParameter("@param12", count));
                            command1.ExecuteNonQuery();
                        }
                        if (checkAddress(address))
                        {
                            SQLiteCommand command2 = new SQLiteCommand("INSERT INTO RentalAddress (Address) VALUES (@param1)", conn);
                            command2.Parameters.Add(new SQLiteParameter("@param1", address));
                            command2.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                   
                    using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=|DataDirectory|\DataFile\RentalDatabase.db"))
                    {
                        conn.Open();
                        if (checkAddress(address))
                        {
                            SQLiteCommand command3 = new SQLiteCommand("INSERT INTO RentalAddress (Address) VALUES (@param1)", conn);
                            command3.Parameters.Add(new SQLiteParameter("@param1", address));
                            command3.ExecuteNonQuery();
                        }
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SQL statement" + ex.ToString());
            }
        }
        public bool checkAddress(string addr)
        {
            int count = 0;
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=|DataDirectory|\DataFile\RentalDatabase.db"))
            {
                conn.Open();
             
                SQLiteCommand command = new SQLiteCommand("SELECT * from RentalAddress WHERE Address="+"'"+addr+"'"+")", conn);
                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    count++;
                }


            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Renter_Button_Click(object sender, RoutedEventArgs e)
        {
            string tenantName, phoneNumber, emailAddress, renewal,renewalin30="NO",renewalin90="NO";
            int rent=0, deposit=0, cleaningDeposit=0, keyDeposit=0;
            string startDate, endDate;
            tenantName = addNameBox.Text;
            phoneNumber = addPhoneNumberBox.Text;
            emailAddress = addEmailBox.Text;
            renewal = addRenewalCombo.Text;
            startDate = addStartDateBox.Text;
            endDate = addEndDateBox.Text;
            bool flag = true,tenantExists=false;
            Renter renter;
            
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            if (tenantName == "")
            {
                flag = false;
                MessageBox.Show("Please enter Tenant Name");
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
            else if (tenantName == "" || renewal == "" || addRentBox.Text == "" || addCleaningDepositBox.Text == "" || addKeyDepositBox.Text == "" || startDate == "" || endDate == "" ||phoneNumber==""||emailAddress=="")
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
            if(renewal== "Renewal in 30")
            {
                renewalin30 = "YES";
            }else if(renewal== " Renewal in 90")
            {
                renewalin90 = "YES";
            }
            else
            {
                renewalin30 = "NO";
                renewalin90 = "NO";
            }
            if (flag)
            {
                

                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=|DataDirectory|\DataFile\RentalDatabase.db"))
                {
                    conn.Open();
                    SQLiteCommand command1 = new SQLiteCommand("Select * from Renter where TenantName ='" + tenantName+"'", conn);
                    SQLiteDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        tenantExists = true;
                        MessageBox.Show("Renter already exists for this address");
                    }

                }
                foreach(Renter r in renterList)
                {
                    if (r.TenantName == tenantName)
                    {
                        tenantExists = true;
                        MessageBox.Show("Renter already exists for this address");
                        
                    }
                }
                if (!tenantExists)
                {
                    renter = new Renter(tenantName, phoneNumber, emailAddress, rent, startDate, endDate, deposit, cleaningDeposit, keyDeposit, renewalin30, renewalin90);
                   
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
    }
}
