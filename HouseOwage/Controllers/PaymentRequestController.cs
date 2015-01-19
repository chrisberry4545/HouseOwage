using HouseOwage.Context;
using HouseOwage.Models;
using System;
using System.Collections.Generic;
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
