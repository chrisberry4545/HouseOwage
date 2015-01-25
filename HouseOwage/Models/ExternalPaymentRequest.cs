using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.Models
{
    public class ExternalPaymentRequest
    {
        public int ExternalPaymentRequestId { get; set; }

        [Display(Name = "Amount (£)")]
        [Range(0.1, 10000)]
        public decimal Amount { get; set; }

        [Display(Name = "Request To")]
        [Required]
        public String RequestTo { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public String Name { get; set; }

        public bool Paid { get; set; }

        public virtual User CreatedBy { get; set; }

        public bool Archived { get; set; }
    }
}
