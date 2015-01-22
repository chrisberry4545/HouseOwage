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
    public class PaymentRequestController : Controller
    {
        //
        // Get: PaymentRequest/Create/
        [HttpGet]
        public ActionResult Create()
        {
            if (Session[HomeController.UserSession] != null)
            {
                PaymentRequest paymentRequest = new PaymentRequest();

                SetUpViewBag();

                return View("Create", paymentRequest);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // Post: PaymentRequest/Create/
        [HttpPost]
        public ActionResult Create(PaymentRequest paymentRequest)
        {
            User user = (User)Session[HomeController.UserSession];
            if (ModelState.IsValid)
            {
                using (var db = new HouseOwageContext())
                {
                    paymentRequest.CreatedBy = db.Users.FirstOrDefault(u => user.UserId == u.UserId);
                    paymentRequest.SentTo = db.Users.FirstOrDefault(u => u.UserId == paymentRequest.SentTo.UserId);
                    db.PaymentRequests.Add(paymentRequest);
                    db.SaveChanges();
                }
                return RedirectToAction("MyDashboard", "Home");
            }
            SetUpViewBag();
            return View("Create", paymentRequest);
        }

        [HttpGet]
        public ActionResult Edit(int paymentRequestId)
        {
            if (Session[HomeController.UserSession] != null)
            {
                using (var db = new HouseOwageContext())
                {
                    var paymentRequest = 
                        db.PaymentRequests
                        .FirstOrDefault(r => r.PaymentRequestId == paymentRequestId);

                    SetUpViewBag();
                    return View("Edit", paymentRequest);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(PaymentRequest paymentRequest)
        {
            User user = (User)Session[HomeController.UserSession];
            if (ModelState.IsValid)
            {
                using (var db = new HouseOwageContext())
                {
                    db.PaymentRequests.Attach(paymentRequest);

                    var entry = db.Entry(paymentRequest);
                    entry.State = EntityState.Modified; 
                    db.SaveChanges();
                }
                return RedirectToAction("MyDashboard", "Home");
            }
            SetUpViewBag();
            return View("Edit", paymentRequest);
        }

        [HttpPost]
        public ActionResult Delete(int paymentRequestId)
        {
            using (var db = new HouseOwageContext())
            {
                var paymentRequest = db.PaymentRequests.FirstOrDefault(p => p.PaymentRequestId == paymentRequestId);
                if (paymentRequest != null)
                {
                    paymentRequest.Archived = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("MyDashboard", "Home");
        }
        private void SetUpViewBag()
        {
            var allUsers = Logic.UserLogic.AllUsers.GetAllUsers();
            User user = (User)Session[HomeController.UserSession];
            if (user != null)
            {
                ViewBag.AllUsers = allUsers.Where(u => u.Text != user.Name); //Current user shouldn't be avaliable
            }
        }

    }
}
