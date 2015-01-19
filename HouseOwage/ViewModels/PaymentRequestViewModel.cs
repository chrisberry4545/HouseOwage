using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.ViewModels
{
    public class PaymentRequestViewModel
    {
        public int PaymentRequestId { get; set; }
        public decimal AmountLeftToPay { get; set; }
        public String RequestName { get; set; }
        public String RequestMadeTo { get; set; }
        public String RequestFrom { get; set; }
        public bool Confirmed { get; set; }
    }
}
