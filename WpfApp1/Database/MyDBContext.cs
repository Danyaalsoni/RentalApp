using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WpfApp1.Database
{
    class MyDBContext:DbContext
    {
        public MyDBContext() : base(new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = "C:\\Users\\danya\\source\repos\\WpfApp1\\WpfApp1\\DataFile\\RentalDatabase.db" }
            .ConnectionString
        }, true)
        {

        }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<RentalAddress> RentalAddresses { get; set; }
    }
}
