using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.ViewModels
{
    public class CreatePaymentRequestViewModel
    {
        public PaymentRequest PaymentRequest { get; set; }
        public List<int> UsersToSentTo { get; set; }
    }
}
