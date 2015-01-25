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
    public class ExternalPaymentRequestController : Controller
    {

        [HttpGet]
        public ActionResult Create()
        {
            var currentUser = (User)Session[HomeController.UserSession];
            if (currentUser != null)
            {
                using (var db = new HouseOwageContext()) 
                {
                    ExternalPaymentRequest paymentRequest = new ExternalPaymentRequest()
                    {
                        CreatedBy = db.Users.FirstOrDefault(u => u.UserId == currentUser.UserId)
                    };
                    return View("Create", paymentRequest);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(ExternalPaymentRequest paymentRequest)
        {
            var currentUser = (User)Session[HomeController.UserSession];
            if (currentUser != null && ModelState.IsValid)
            {
                using (var db = new HouseOwageContext())
                {
                    paymentRequest.Created = DateTime.Now;
                    paymentRequest.CreatedBy = db.Users.FirstOrDefault(u => u.UserId == currentUser.UserId);
                    db.ExternalPaymentRequests.Add(paymentRequest);
                    db.SaveChanges();
                    return RedirectToAction("MyDashboard", "Home");
                }
            }
            return View("Create", paymentRequest);
        }

        [HttpGet]
        public ActionResult Edit(int paymentRequestId)
        {
            var currentUser = (User)Session[HomeController.UserSession];
            if (currentUser != null)
            {
                using (var db = new HouseOwageContext())
                {
                    var externalPaymentRequest = db.ExternalPaymentRequests
                        .Include("CreatedBy").FirstOrDefault(r => r.ExternalPaymentRequestId == paymentRequestId);
                    if (externalPaymentRequest != null)
                    {
                        return View("Edit", externalPaymentRequest);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(ExternalPaymentRequest paymentRequest)
        {
            var currentUser = (User)Session[HomeController.UserSession];
            paymentRequest.CreatedBy = currentUser;
            if (currentUser != null && ModelState.IsValid)
            {
                using (var db = new HouseOwageContext())
                {
                    db.ExternalPaymentRequests.Attach(paymentRequest);
                    var entry = db.Entry(paymentRequest);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("MyDashboard", "Home");
            }
            return View("Edit", paymentRequest);
        }


        [HttpPost]
        public ActionResult Delete(int paymentRequestId)
        {
            var currentUser = (User)Session[HomeController.UserSession];
            if (currentUser != null)
            {
                using (var db = new HouseOwageContext())
                {
                    var paymentRequest = db.ExternalPaymentRequests.FirstOrDefault(p => p.ExternalPaymentRequestId == paymentRequestId
                        && p.CreatedBy.UserId == currentUser.UserId);
                    if (paymentRequest != null)
                    {
                        paymentRequest.Archived = true;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("MyDashboard", "Home");
        }

    }
}
