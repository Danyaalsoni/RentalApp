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
    [Table("RentalAddress")]
    class RentalAddress
    {
        public int ID { get; set; }
        public string address { get; set; }
        public int renterID { get; set; }
    }
}
