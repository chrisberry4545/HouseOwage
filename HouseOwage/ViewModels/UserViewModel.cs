using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.ViewModels
{
    public class UserViewModel : IValidatableObject
    {
        [Display(Name = "Current Password")]
        public String CurrentPassword { get; set; }
        [Display(Name = "New Password")]
        public String NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        public String ConfirmNewPassword { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewPassword.Equals(ConfirmNewPassword))
            {
                yield return new ValidationResult("Passwords do not match.", new List<String>() { "NewPassword" });
            }
        }
    }
}
