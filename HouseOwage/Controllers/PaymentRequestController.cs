using HouseOwage.Context;
using HouseOwage.Models;
using HouseOwage.ViewModels;
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
                CreatePaymentRequestViewModel vm = new CreatePaymentRequestViewModel();
                vm.PaymentRequest = paymentRequest;
                SetUpViewBag();

                var allUsers = (IEnumerable<SelectListItem>)ViewBag.AllUsers;
                vm.UsersToSentTo = new List<int>();
                for (int i = 0; i < allUsers.Count(); i++)
                {
                    vm.UsersToSentTo.Add(-1);
                }

                return View("Create", vm);
            }
            return RedirectToAction("Index", "Home");
        }
        //
        // Post: PaymentRequest/Create/
        [HttpPost]
        public ActionResult Create(CreatePaymentRequestViewModel createPaymentRequestVM)
        {
            User user = (User)Session[HomeController.UserSession];
            if (user != null && ModelState.IsValid)
            {
                using (var db = new HouseOwageContext())
                {
                    foreach (var userId in createPaymentRequestVM.UsersToSentTo)
                    {
                        if (userId != -1)
                        {
                            var createdBy = db.Users.FirstOrDefault(u => user.UserId == u.UserId);
                            if (createdBy != null)
                            {
                                PaymentRequest paymentRequest = new PaymentRequest()
                                {
                                    Amount = createPaymentRequestVM.PaymentRequest.Amount,
                                    Created = DateTime.Now,
                                    CreatedBy = createdBy,
                                    SentTo_UserId = userId,
                                    Name = createPaymentRequestVM.PaymentRequest.Name
                                };
                                db.PaymentRequests.Add(paymentRequest);
                            }
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("MyDashboard", "Home");
                }
            }
            SetUpViewBag();
            return View("Create", createPaymentRequestVM);
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
            if (ModelState.IsValid && user != null)
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
                var allUsersWithBlank = new List<SelectListItem>();
                allUsersWithBlank.Add(new SelectListItem { Value = "-1", Text = "None"});
                allUsersWithBlank.AddRange(ViewBag.AllUsers);
                ViewBag.AllUsersWithBlank = allUsersWithBlank;
            }
        }

    }
}
