using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace RentalApp.Database
{
    
    class RentalAddress
    {
        public int ID { get; set; }
        public string address { get; set; }
        public int renterID { get; set; }
        public int renters { get; set; }
        string dbloc;
        public RentalAddress (int ID,string address){
            this.ID = ID;
            this.address = address;
            this.renterID = renterID;
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
                this.renters = 0;
                SQLiteCommand command1 = new SQLiteCommand("Select * from Renter where AddressID="+this.ID.ToString(), conn);
                SQLiteDataReader reader = command1.ExecuteReader();
                if (!reader.HasRows)
                {
                    this.renters = 0;
                }
                else
                {
                    while (reader.Read())
                    {
                        this.renters += 1;
                    }
                }
            }
        }
        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
