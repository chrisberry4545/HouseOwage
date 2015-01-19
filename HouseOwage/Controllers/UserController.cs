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
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            var user = (User)Session[HomeController.UserSession];
            if (user.Admin)
            {
                return View("Create", new User());
            }
            return RedirectToAction("MyDashboard", "Home");
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new HouseOwageContext())
                {
                    user.Password = Logic.UserLogic.Passwords.GenerateKeyHash(user.Password);

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("MyDashboard", "Home");
                }
            }
            return View("Create", user);
        }

        //
        // GET: /User/
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session[HomeController.UserSession] != null)
            {
                return View(new UserViewModel());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ChangePassword(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(new UserViewModel());
            }

            var user = (User)Session[HomeController.UserSession];
            if (user != null)
            {
                using (var db = new HouseOwageContext())
                {
                    var dbUser = Logic.UserLogic.Login.LoginUser(user.UserName, userViewModel.CurrentPassword);

                    if (dbUser == null)
                    {
                        ViewBag.PasswordIncorrect = true;
                        return View(new UserViewModel());
                    }
                    else
                    {
                        dbUser.Password = Logic.UserLogic.Passwords.GenerateKeyHash(userViewModel.NewPassword);
                        db.Entry(dbUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
