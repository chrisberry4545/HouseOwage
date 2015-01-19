using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.Context
{
    public class HouseOwageContext : DbContext
    {
        public HouseOwageContext()
        {
#if !DEBUG
            String connString = ConfigurationManager.AppSettings["dbConnString"].ToString();
            this.Database.Connection.ConnectionString = connString;
#endif
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
