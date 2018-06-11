using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1
{
    [Table("Renter")]
    public class Renter
    {
        public int ID { get; set; }
        public string tenantName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public double rent { get; set; }
        public DateTime startDate { get; set; }
        public DateTime termEnd { get; set; }
        public double deposit { get; set; }
        public double cleaningDeposit { get; set; }
        public double keyDeposit { get; set; }

    }
}
