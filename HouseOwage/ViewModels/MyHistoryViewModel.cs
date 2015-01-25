using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.ViewModels
{
    public class MyHistoryViewModel
    {
        public IEnumerable<PaymentViewModel> MyConfirmedPayments { get; set; }
        public IEnumerable<PaymentRequestViewModel> MyHonouredRequests { get; set; }
    }
}
