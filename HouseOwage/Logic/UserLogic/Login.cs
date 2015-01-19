using HouseOwage.Context;
using HouseOwage.Models;
using HouseOwage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.Logic.UserLogic
{
    public class Login
    {
        public static MyDashboardViewModel GetDashboardVMForUser(User user)
        {
            using (var db = new HouseOwageContext()) 
            {
                MyDashboardViewModel vm = new MyDashboardViewModel();
                vm.UserDetails = user; 

                var myPayments = db.Payments
                    .Include("PaymentRequest")
                    .Where(p => p.PaymentRequest.SentTo.UserId == vm.UserDetails.UserId)
                    .ToList();
                var confirmedPaymentsToMe = db.Payments
                    .Include("PaymentRequest")
                    .Where(p => p.PaymentRequest.CreatedBy.UserId == vm.UserDetails.UserId && p.Confirmed)
                    .ToList();

                //My requests
                var myRequests = db.PaymentRequests.Include("SentTo").Where(r => r.CreatedBy.UserId == vm.UserDetails.UserId).ToList();

                List<PaymentRequestViewModel> myUnresolvedRequests = new List<PaymentRequestViewModel>();
                List<PaymentRequestViewModel> myResolvedRequests = new List<PaymentRequestViewModel>();
                foreach(var req in myRequests) 
                {
                    decimal paymentsToRequest = confirmedPaymentsToMe.Where(p => req.PaymentRequestId == p.PaymentRequest.PaymentRequestId).Sum(p => p.Amount);
                    decimal amountLeftToPay = req.Amount - paymentsToRequest;

                    PaymentRequestViewModel payReqVM = new PaymentRequestViewModel ()
                    {
                        PaymentRequestId = req.PaymentRequestId,
                        AmountLeftToPay = amountLeftToPay,
                        RequestMadeTo = req.SentTo.Name,
                        RequestName = req.Name,
                        RequestFrom = vm.UserDetails.Name
                    };

                    if (amountLeftToPay > 0) {
                        myUnresolvedRequests.Add(payReqVM);
                    } else {
                        myResolvedRequests.Add(payReqVM);
                    }
                }

                vm.MyUnresolvedRequests = myUnresolvedRequests;
                vm.MyResolvedRequests = myResolvedRequests;

                //Requests made to me
                var requestsMadeToMe = db.PaymentRequests.Include("CreatedBy").Where(r => r.SentTo.UserId == vm.UserDetails.UserId).ToList();

                List<PaymentRequestViewModel> requestsSentToMeAndPaid = new List<PaymentRequestViewModel>();
                List<PaymentRequestViewModel> requestsSentToMeAndUnpaid = new List<PaymentRequestViewModel>();
                foreach (var req in requestsMadeToMe)
                {
                    decimal paymentsToRequest = myPayments.Where(p => req.PaymentRequestId == p.PaymentRequest.PaymentRequestId).Sum(p => p.Amount);
                    decimal amountLeftToPay = req.Amount - paymentsToRequest;

                    PaymentRequestViewModel payReqVM = new PaymentRequestViewModel()
                    {
                        PaymentRequestId = req.PaymentRequestId,
                        AmountLeftToPay = amountLeftToPay,
                        RequestMadeTo = vm.UserDetails.Name,
                        RequestName = req.Name,
                        RequestFrom = req.CreatedBy.Name
                    };

                    if (amountLeftToPay > 0)
                    {
                        requestsSentToMeAndUnpaid.Add(payReqVM);
                    }
                    else
                    {
                        requestsSentToMeAndPaid.Add(payReqVM);
                    }
                }

                vm.RequestsSentToMeAndPaid = requestsSentToMeAndPaid;
                vm.RequestsSentToMeAndUnpaid = requestsSentToMeAndUnpaid;

                vm.PaymentsRequiringConfirmation = db.Payments.Where(p => p.PaymentRequest.CreatedBy.UserId == vm.UserDetails.UserId && !p.Confirmed)
                    .Select(p => new PaymentViewModel()
                    {
                        AmountPaid = p.Amount,
                        Confirmed = false,
                        PaymentId = p.PaymentId,
                        PaymentMadeBy = p.PaymentRequest.SentTo.Name,
                        PaymentMadeTo = p.PaymentRequest.CreatedBy.Name,
                        PaymentRequestName = p.PaymentRequest.Name
                    }).ToList();

                vm.MyUnconfirmedPayments = db.Payments.Where(p => p.PaymentRequest.SentTo.UserId == vm.UserDetails.UserId && !p.Confirmed)
                    .Select(p => new PaymentViewModel()
                    {
                        AmountPaid = p.Amount,
                        Confirmed = false,
                        PaymentId = p.PaymentId,
                        PaymentMadeBy = p.PaymentRequest.SentTo.Name,
                        PaymentMadeTo = p.PaymentRequest.CreatedBy.Name,
                        PaymentRequestName = p.PaymentRequest.Name
                    }).ToList();

                return vm;
            }
        }


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
