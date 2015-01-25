using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HouseOwage.ViewModels
{
    public class MyDashboardViewModel
    {
        public User UserDetails { get; set; }

        public IEnumerable<PaymentRequestViewModel> MyResolvedRequests { get; set; }
        public IEnumerable<PaymentRequestViewModel> MyUnresolvedRequests { get; set; }

        public IEnumerable<PaymentRequestViewModel> RequestsSentToMeAndUnpaid { get; set; }
        public IEnumerable<PaymentRequestViewModel> RequestsSentToMeAndPaid { get; set; }

        public IEnumerable<PaymentViewModel> PaymentsRequiringConfirmation { get; set; }
        public IEnumerable<PaymentViewModel> MyUnconfirmedPayments { get; set; }

        public IEnumerable<SelectListItem> UserNames { get; set; }
    }
}
