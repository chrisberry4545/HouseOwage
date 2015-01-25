using HouseOwage.Context;
using HouseOwage.Models;
using HouseOwage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseOwage.Controllers
{
    public class HistoryController : Controller
    {
        //
        // GET: /History/

        public ActionResult Index()
        {
            User user = (User)Session[HomeController.UserSession];
            if (user != null)
            {
                return View("Index", GenerateHistoryViewModel(user));
            }

            return RedirectToAction("MyDashboard", "Home");
        }

        private static MyHistoryViewModel GenerateHistoryViewModel(User user) 
        {
            MyHistoryViewModel historyViewModel = new MyHistoryViewModel();

            using (var db = new HouseOwageContext())
            {
                var myConfirmedPayments = db.Payments.Where(p => p.PaymentRequest.SentTo_UserId == user.UserId && p.Confirmed)
                    .Select(p => new PaymentViewModel() {
                        AmountPaid = p.Amount,
                        Confirmed = p.Confirmed,
                        Created = p.Created,
                        PaymentId = p.PaymentId,
                        PaymentMadeBy = user.Name,
                        PaymentMadeTo = p.PaymentRequest.CreatedBy.Name,
                        PaymentRequestName = p.PaymentRequest.Name
                    })
                    .ToList();
                historyViewModel.MyConfirmedPayments = myConfirmedPayments;

                var allConfirmedPaymentsToMe = db.Payments.Include("PaymentRequest").Where(p => p.Confirmed && p.PaymentRequest.CreatedBy.UserId == user.UserId).ToList();

                var myPaymentRequests = db.PaymentRequests
                    .Include("SentTo")
                    .Where(r => r.CreatedBy.UserId == user.UserId);

                List<PaymentRequestViewModel> myCompletedPaymentRequests = new List<PaymentRequestViewModel>();
                foreach(var paymentRequest in myPaymentRequests) 
                {
                    decimal amountPaid = allConfirmedPaymentsToMe
                        .Where(p => p.PaymentRequest.PaymentRequestId == paymentRequest.PaymentRequestId).Sum(p => p.Amount);
                    if (amountPaid >= paymentRequest.Amount) {
                        myCompletedPaymentRequests.Add(new PaymentRequestViewModel() 
                        {
                            Confirmed = true,
                            AmountLeftToPay = paymentRequest.Amount,
                            Created = paymentRequest.Created,
                            PaymentRequestId = paymentRequest.PaymentRequestId,
                            RequestMadeTo = paymentRequest.SentTo.Name,
                            RequestFrom = user.Name,
                            RequestName = paymentRequest.Name
                        });
                    }
                }

                historyViewModel.MyHonouredRequests = myCompletedPaymentRequests;
                return historyViewModel;
            }
        }

    }
}
