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
    /// Interaction logic for AddRentalAddress.xaml
    /// </summary>
    public partial class AddRentalAddress : Window
    {
        List<Renter> renterList = new List<Renter>();
        string dbloc;
        int addressID;
        public AddRentalAddress()
        {
            InitializeComponent();
            dbloc = ((MainWindow)Application.Current.MainWindow).dbloc;
            addNameBox.IsEnabled = false;
            addPhoneNumberBox.IsEnabled = false;
            addEmailBox.IsEnabled = false;
            addRentBox.IsEnabled = false;
            addStartDateBox.IsEnabled = false;
            addEndDateBox.IsEnabled = false;
            addDepositBox.IsEnabled = false;
            addDepositDateBox.IsEnabled = false;
            addCleaningDepositBox.IsEnabled = false;
            addCleaningDateBox.IsEnabled = false;
            addKeyDateBox.IsEnabled = false;
            addKeyDepositBox.IsEnabled = false;
            addRenewalCombo.IsEnabled = false;
            addrentDateBox.IsEnabled = false;
        }

        private void Add_Address_Click(object sender, RoutedEventArgs e)
        {
            string address = addstreetAddressBox.Text;
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }

            if (address == "")
            {
                MessageBox.Show("Please Enter Street Address");
                return;
            }
            if (checkAddress(address))
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                {
                    conn.Open();
                    SQLiteCommand command2 = new SQLiteCommand("INSERT INTO RentalAddress (Address) VALUES (@param1)", conn);
                    command2.Parameters.Add(new SQLiteParameter("@param1", address));
                    command2.ExecuteNonQuery();
                }
            }
            addstreetAddressBox.IsEnabled = false;
            Button bt = (Button)sender;
            bt.IsEnabled = false;
            addNameBox.IsEnabled = true;
            addPhoneNumberBox.IsEnabled = true;
            addEmailBox.IsEnabled = true;
            addRentBox.IsEnabled = true;
            addStartDateBox.IsEnabled = true;
            addEndDateBox.IsEnabled = true;
            addDepositBox.IsEnabled = true;
            addDepositDateBox.IsEnabled = true;
            addCleaningDepositBox.IsEnabled = true;
            addCleaningDateBox.IsEnabled = true;
            addKeyDateBox.IsEnabled = true;
            addKeyDepositBox.IsEnabled = true;
            addRenewalCombo.IsEnabled = true;
            addrentDateBox.IsEnabled=true;
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            string address = addstreetAddressBox.Text;
            int count = 0;
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }
           
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                {
                    
                    conn.Open();
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand("SELECT * from RentalAddress WHERE Address="+"'"+address+"'", conn);
                        SQLiteDataReader reader1 = command.ExecuteReader();

                        while (reader1.Read())
                        {
                            count=Convert.ToInt32(reader1["ID"]);
                            addressID = count;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error in select all rental address statement");
                    }

                }
                if (renterList.Count != 0)
                {
                    
                    using (SQLiteConnection conn = new SQLiteConnection(dbloc))
                    {
                        conn.Open();
                        foreach (Renter renter in renterList)
                        {
                            SQLiteCommand command1 = new SQLiteCommand("INSERT INTO Renter (TenantName,Phone,Email,Rent,StartDate,EndDate,Deposit,CleaningDeposit,KeyDeposit,Renewal_in_30,Renewal_in_90,AddressID,CleaningDepositDate,KeyDepositDate,DepositDate,RentDate) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14,@param15,@param16)", conn);
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
                            command1.Parameters.Add(new SQLiteParameter("@param13", renter.cleaningDepositDate));
                            command1.Parameters.Add(new SQLiteParameter("@param14", renter.keyDepositDate));
                            command1.Parameters.Add(new SQLiteParameter("@param15", renter.depositDate));
                            command1.Parameters.Add(new SQLiteParameter("@param16", renter.rentDueDate));
                            command1.ExecuteNonQuery();
                        }
                        
                    }
                }

                MessageBox.Show("Saved successfully");
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
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {
                conn.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand("SELECT * from RentalAddress WHERE Address=" + "'" + addr + "'", conn);
                    SQLiteDataReader reader1 = command.ExecuteReader();

                    while (reader1.Read())
                    {
                        count++;
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
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
            string tenantName, phoneNumber, emailAddress, renewal,renewalin30="NO",renewalin90="NO",depositDate="",cleaningDepositDate="",keyDepositDate="", rentDueDate;
            int rent=0, deposit=0, cleaningDeposit=0, keyDeposit=0;
            bool flag = true, tenantExists = false;
            string startDate, endDate;
            tenantName = addNameBox.Text;
            phoneNumber = addPhoneNumberBox.Text;
            emailAddress = addEmailBox.Text;
            renewal = "No Renewal";
            try
            {
                renewal = addRenewalCombo.SelectedValue.ToString();
                flag = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select value for renewal");
            }
            startDate = addStartDateBox.Text;
            endDate = addEndDateBox.Text;
            depositDate = addDepositDateBox.Text;
            cleaningDepositDate = addCleaningDateBox.Text;
            keyDepositDate = addKeyDateBox.Text;
            rentDueDate = addrentDateBox.Text;
            
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
            else if (rentDueDate == "")
            {
                flag = false;
                MessageBox.Show("Please enter Rent Due Date");
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
            else if (rentDueDate==""|| cleaningDepositDate==""||depositDate==""||keyDepositDate==""|| tenantName == "" || renewal == "" || addRentBox.Text == "" || addCleaningDepositBox.Text == "" || addKeyDepositBox.Text == "" || startDate == "" || endDate == "" ||phoneNumber==""||emailAddress=="")
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
                renewalin90 = "NO";
            }else if(renewal== "Renewal in 90")
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
                    SQLiteCommand command1 = new SQLiteCommand("Select * from Renter where TenantName ='" + tenantName+"'" + "AND AddressID="+ addressID, conn);
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
                    renter = new Renter(tenantName, phoneNumber, emailAddress, rent, startDate, endDate, deposit, cleaningDeposit, keyDeposit, renewalin30, renewalin90,depositDate,keyDepositDate,cleaningDepositDate,rentDueDate);
                   
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
