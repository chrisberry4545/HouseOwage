using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.Models
{
    public class User
    {
        public int UserId { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public bool Admin { get; set; }
        public String Salt { get; set; }

        public virtual IEnumerable<PaymentRequest> MyRequests { get; set; }
        public virtual IEnumerable<Payment> MyPayments { get; set; }

        public virtual IEnumerable<PaymentRequest> RequestSentToMe { get; set; }
    }
}
