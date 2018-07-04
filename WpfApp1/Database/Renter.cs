using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalApp
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
        public string Renewal { get; set; }
        public string cleaningDepositDate { get; set; }
        public string keyDepositDate { get; set; }
        public string depositDate { get; set; }
        public string rentDueDate { get; set; }

        public Renter(string name,string phone,string email,double rent,string start,string end,double deposit,double cleaningDeposit,double keyDeposit,string ren30,string ren90,string depositDate,string keyDepositDate,string cleaningDepositDate,string rentDueDate)
        {
            //this.ID = ID;
            this.TenantName = name;
            this.Phone = phone;
            this.Email = email;
            this.Rent = rent;
            this.StartDate = start;
            this.EndDate = end;
            this.deposit = deposit;
            this.cleaningDeposit = cleaningDeposit;
            this.keyDeposit = keyDeposit;
            this.cleaningDepositDate = cleaningDepositDate;
            this.depositDate = depositDate;
            this.keyDepositDate = keyDepositDate;
            this.Renewal_in_30 = ren30;
            this.Renewal_in_90 = ren90;
            this.rentDueDate = rentDueDate;
            if (ren30 == "YES" && ren90 == "NO")
            {
                this.Renewal = "Renewal in 30 Days";
            }else if (ren90=="YES" && ren30 == "NO")
            {
                this.Renewal = "Renewal in 90 Days";
            }
            else
            {
                this.Renewal = "No Renewal";
            }
        }

        public Renter(int ID,string name, string phone, string email, double rent, string start, string end, double deposit, double cleaningDeposit, double keyDeposit, string ren30, string ren90, string depositDate, string keyDepositDate, string cleaningDepositDate, string rentDueDate)
        {
            this.ID = ID;
            this.TenantName = name;
            this.Phone = phone;
            this.Email = email;
            this.Rent = rent;
            this.StartDate = start;
            this.EndDate = end;
            this.deposit = deposit;
            this.cleaningDeposit = cleaningDeposit;
            this.keyDeposit = keyDeposit;
            this.cleaningDepositDate = cleaningDepositDate;
            this.depositDate = depositDate;
            this.keyDepositDate = keyDepositDate;
            this.Renewal_in_30 = ren30;
            this.Renewal_in_90 = ren90;
            this.rentDueDate = rentDueDate;
            if (ren30 == "YES")
            {
                this.Renewal = "Renewal in 30 Days";
            }
            else if (ren90 == "YES")
            {
                this.Renewal = "Renewal in 90 Days";
            }
            else
            {
                this.Renewal = "No Renewal";
            }
        }
    }
}
