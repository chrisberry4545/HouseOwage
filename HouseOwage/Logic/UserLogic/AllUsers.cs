using HouseOwage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HouseOwage.Logic.UserLogic
{
    public class AllUsers
    {


        public static IEnumerable<SelectListItem> GetAllUsers()
        {
            using (var db = new HouseOwage.Context.HouseOwageContext())
            {
                var allUsers = db.Users.Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.UserId.ToString()
                }).ToList();
                return allUsers;
            }
        }
 
    }

}
