using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public decimal AmountPaid { get; set; }
        public String PaymentMadeBy { get; set; }
        public String PaymentMadeTo { get; set; }
        public bool Confirmed { get; set; }
        public String PaymentRequestName { get; set; }
        public DateTime Created { get; set; }
    }
}
