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
   
    public class Renter
    {
        public int ID { get; set; }
        public string TenantName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Rent { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double deposit { get; set; }
        public double cleaningDeposit { get; set; }
        public double keyDeposit { get; set; }
        public int AddressID { get; set; }
        public string Renewal_in_30 { get; set; }
        public string Renewal_in_90 { get; set; }

        public Renter(int ID,string name,string phone,string email,double rent,DateTime start,DateTime end,double deposit,double cleaningDeposit,double keyDeposit,string ren30,string ren90)
        {
            this.ID = ID;
            this.TenantName = name;
            this.Phone = phone;
            this.Email = email;
            this.Rent = rent;
            this.StartDate = start.ToString();
            this.EndDate = end.ToString();
            this.deposit = deposit;
            this.cleaningDeposit = cleaningDeposit;
            this.keyDeposit = keyDeposit;

        }
      

    }
}
