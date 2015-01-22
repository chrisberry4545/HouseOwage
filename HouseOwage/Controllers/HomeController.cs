using HouseOwage.Context;
using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseOwage.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string UserSession = "User";

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MyDashboard()
        {
            var user = (User)Session[UserSession];
            if (user != null)
            {
                return MyDashboard(user);
            }

            return View("Index", new User());
        }


        // Post: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyDashboard([Bind(Include = "UserName,Password")]User user)
        {
            var userLoggedIn = (User)Session[UserSession];

            if (userLoggedIn == null)
            {
                userLoggedIn = Logic.UserLogic.Login.LoginUser(user.UserName, user.Password);
            }

            if (userLoggedIn != null)
            {
                var dashboardVM = Logic.UserLogic.Login.GetDashboardVMForUser(userLoggedIn);
                Session[UserSession] = userLoggedIn;
                return View("MyDashboard", dashboardVM);
            }
            ViewBag.LoginError = "Username or password is incorrect";
            return View("Index", user);
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Session[UserSession] = null;
            return RedirectToAction("Index");
        }


    }
}
