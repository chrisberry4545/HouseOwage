using HouseOwage.Context;
using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseOwage.Controllers
{
    public class PaymentController : Controller
    {
        //
        // Post: Payment/New/
        [HttpPost]
        public ActionResult New(int requestId)
        {
            using (var db = new HouseOwageContext())
            {
                var paymentRequest = db.PaymentRequests.FirstOrDefault(r => r.PaymentRequestId == requestId);
                if (paymentRequest != null)
                {
                    Payment payment = new Payment()
                    {
                        Amount = paymentRequest.Amount,
                        Confirmed = false,
                        PaymentRequest = paymentRequest
                    };
                    return View("Create", payment);
                }

            }

            return View();

        }

        //
        // Post: Payment/Create/
        [HttpPost]
        public ActionResult Create(Payment payment)
        {
            //if (ModelState.IsValid)
            //{
                using (var db = new HouseOwageContext())
                {
                    payment.PaymentRequest = db.PaymentRequests.FirstOrDefault(r => r.PaymentRequestId == payment.PaymentRequest.PaymentRequestId);
                    db.Payments.Add(payment);
                    db.SaveChanges();
                }
                return RedirectToAction("MyDashboard", "Home");
            //}
            //return View("Create", payment);
        }


        [HttpPost]
        public ActionResult ConfirmPayment(int paymentId)
        {
            //Check user is payment receiver
            var currentUser = (User)Session[HomeController.UserSession];

            using (var db = new HouseOwageContext())
            {
                var payment = db.Payments.FirstOrDefault(p => p.PaymentId == paymentId && p.PaymentRequest.CreatedBy.UserId == currentUser.UserId);
                if (payment != null)
                {
                    payment.Confirmed = true;
                    db.SaveChanges();
                }
            }
            //TODO::No error thrown if user is not the correct person to confirm payments or the payment cannot be found.
            return RedirectToAction("MyDashboard", "Home");
        }

    }
}
