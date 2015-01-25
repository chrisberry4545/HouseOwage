using HouseOwage.Context;
using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    payment.Created = DateTime.Now;
                    db.Payments.Add(payment);
                    db.SaveChanges();
                }
                return RedirectToAction("MyDashboard", "Home");
            //}
            //return View("Create", payment);
        }

        [HttpGet]
        public ActionResult Edit(int paymentId)
        {
            if (Session[HomeController.UserSession] != null)
            {
                using (var db = new HouseOwageContext())
                {
                    var payment =
                        db.Payments
                        .Include("PaymentRequest")
                        .FirstOrDefault(r => r.PaymentId == paymentId);
                    return View("Edit", payment);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(Payment payment)
        {
            User user = (User)Session[HomeController.UserSession];
            if (ModelState.IsValid && user != null)
            {
                using (var db = new HouseOwageContext())
                {
                    db.Payments.Attach(payment);
                    var entry = db.Entry(payment);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("MyDashboard", "Home");
            }
            return View("Edit", payment);
        }

        [HttpPost]
        public ActionResult Delete(int paymentId)
        {
            using (var db = new HouseOwageContext())
            {
                var payment = db.Payments.FirstOrDefault(p => p.PaymentId == paymentId);
                if (payment != null)
                {
                    payment.Archived = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("MyDashboard", "Home");
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

        [HttpPost]
        public ActionResult ClearPayments(List<int> paymentRequestIds)
        {
            var currentUser = (User)Session[HomeController.UserSession];
            if (currentUser != null)
            {
                using (var db = new HouseOwageContext())
                {
                    var allPaymentRequests = db.PaymentRequests
                        .Where(r => paymentRequestIds.Contains(r.PaymentRequestId) && r.SentTo_UserId == currentUser.UserId)
                        .ToList();
                    foreach(var req in allPaymentRequests) 
                    {
                        Payment payment = new Payment()
                        {
                            Amount = req.Amount,
                            Created = DateTime.Today,
                            PaymentRequest = req
                        };
                        db.Payments.Add(payment);
                    }
                    db.SaveChanges();
                    return Content("Ok");
                }
            }
            return Content("Fail");
        }

    }
}
