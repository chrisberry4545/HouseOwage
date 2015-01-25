using HouseOwage.Context;
using HouseOwage.Models;
using HouseOwage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HouseOwage.Logic.UserLogic
{
    public class Login
    {

        public static readonly string UserSession = "Username";
        public static User LoginUser(string username, string password)
        {
            using (var db = new HouseOwageContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName.ToUpper().Equals(username.ToUpper()));
                //Check password is ok.
                if (user != null && UserLogic.Passwords.ComparePasswords(user.Password, password))
                {
                    return user;
                }
                return null;
            }
        }

        public static User GetFullUserDetails(User basicDetails)
        {
            using (var db = new HouseOwageContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.UserName.ToUpper().Equals(basicDetails.UserName.ToUpper()) && u.Password.Equals(basicDetails.Password));

                if (user != null)
                {
                    user.RequestSentToMe = db.PaymentRequests.Include("CreatedBy").Where(r => r.SentTo.UserId == user.UserId).ToList();
                    user.MyRequests = db.PaymentRequests.Include("SentTo").Where(r => r.CreatedBy.UserId == user.UserId).ToList();
                    user.MyPayments = db.Payments.Include("PaymentRequest.CreatedBy").Where(p => p.PaymentRequest.SentTo.UserId == user.UserId).ToList();
                }

                return user;
            }
        }

        public static bool IsUser(User user)
        {
            return IsUser(user.UserName, user.Password);
        }

        public static bool IsUser(String userName, String password)
        {
            using (var db = new HouseOwageContext()) 
            {
                return db.Users.Any(u => u.UserName.ToUpper().Equals(userName.ToUpper()) && u.Password.Equals(password));
            }
        }
    }
}
