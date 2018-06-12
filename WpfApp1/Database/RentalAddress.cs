using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WpfApp1.Database
{
    
    class RentalAddress
    {
        public int ID { get; set; }
        public string address { get; set; }
        public int renterID { get; set; }
        public int renters { get; set; }
        public RentalAddress (int ID,string address,int renterID){
            this.ID = ID;
            this.address = address;
            this.renterID = renterID;
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\danya\source\repos\WpfApp1\WpfApp1\DataFile\RentalDatabase.db;"))
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
