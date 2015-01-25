using HouseOwage.Context;
using HouseOwage.Models;
using HouseOwage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HouseOwage.Logic
{
    public class DashboardSetup
    {

        public static MyDashboardViewModel GetDashboardVMForUser(User user)
        {
            using (var db = new HouseOwageContext())
            {
                MyDashboardViewModel vm = new MyDashboardViewModel();
                vm.UserDetails = user;

                var myPayments = db.Payments
                    .Include("PaymentRequest")
                    .Where(p => p.PaymentRequest.SentTo.UserId == vm.UserDetails.UserId
                    && !p.Archived)
                    .ToList();
                var confirmedPaymentsToMe = db.Payments
                    .Include("PaymentRequest")
                    .Where(p => p.PaymentRequest.CreatedBy.UserId == vm.UserDetails.UserId && p.Confirmed
                    && !p.Archived)
                    .ToList();

                //My requests
                var myRequests = db.PaymentRequests.Include("SentTo")
                    .Where(r => r.CreatedBy.UserId == vm.UserDetails.UserId
                            && !r.Archived).ToList();

                List<PaymentRequestViewModel> myUnresolvedRequests = new List<PaymentRequestViewModel>();
                List<PaymentRequestViewModel> myResolvedRequests = new List<PaymentRequestViewModel>();
                foreach (var req in myRequests)
                {
                    decimal paymentsToRequest = confirmedPaymentsToMe.Where(p => req.PaymentRequestId == p.PaymentRequest.PaymentRequestId).Sum(p => p.Amount);
                    decimal amountLeftToPay = req.Amount - paymentsToRequest;

                    PaymentRequestViewModel payReqVM = new PaymentRequestViewModel()
                    {
                        PaymentRequestId = req.PaymentRequestId,
                        AmountLeftToPay = amountLeftToPay,
                        RequestMadeTo = req.SentTo.Name,
                        RequestName = req.Name,
                        RequestFrom = vm.UserDetails.Name,
                        Created = req.Created
                    };

                    if (amountLeftToPay > 0)
                    {
                        myUnresolvedRequests.Add(payReqVM);
                    }
                    else
                    {
                        myResolvedRequests.Add(payReqVM);
                    }
                }

                vm.MyUnresolvedRequests = myUnresolvedRequests;
                vm.MyResolvedRequests = myResolvedRequests;

                //Requests made to me
                var requestsMadeToMe = db.PaymentRequests.Include("CreatedBy")
                    .Where(r => r.SentTo.UserId == vm.UserDetails.UserId
                    && !r.Archived).ToList();

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
                        RequestFrom = req.CreatedBy.Name,
                        Created = req.Created
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

                vm.PaymentsRequiringConfirmation = db.Payments.Where(p => p.PaymentRequest.CreatedBy.UserId == vm.UserDetails.UserId && !p.Confirmed && !p.Archived)
                    .Select(p => new PaymentViewModel()
                    {
                        AmountPaid = p.Amount,
                        Confirmed = false,
                        PaymentId = p.PaymentId,
                        PaymentMadeBy = p.PaymentRequest.SentTo.Name,
                        PaymentMadeTo = p.PaymentRequest.CreatedBy.Name,
                        PaymentRequestName = p.PaymentRequest.Name,
                        Created = p.Created
                    }).ToList();

                vm.MyUnconfirmedPayments = db.Payments.Where(p => p.PaymentRequest.SentTo.UserId == vm.UserDetails.UserId && !p.Confirmed && !p.Archived)
                    .Select(p => new PaymentViewModel()
                    {
                        AmountPaid = p.Amount,
                        Confirmed = false,
                        PaymentId = p.PaymentId,
                        PaymentMadeBy = p.PaymentRequest.SentTo.Name,
                        PaymentMadeTo = p.PaymentRequest.CreatedBy.Name,
                        PaymentRequestName = p.PaymentRequest.Name,
                        Created = p.Created
                    }).ToList();

                List<SelectListItem> usersSelectList = new List<SelectListItem>();
                usersSelectList.Add(new SelectListItem()
                {
                    Text = "All",
                    Value = "All",
                    Selected = true
                });
                usersSelectList.AddRange(db.Users
                    .Where(u => u.UserId != vm.UserDetails.UserId)
                    .Select(u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = u.Name
                    }).ToList());

                vm.UserNames = usersSelectList;

                vm.ExternalPaymentRequests = db.ExternalPaymentRequests
                    .Where(r => r.CreatedBy.UserId == vm.UserDetails.UserId && !r.Paid && !r.Archived).ToList();

                return vm;
            }
        }

    }
}
