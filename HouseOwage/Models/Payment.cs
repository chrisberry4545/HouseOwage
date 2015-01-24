using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Display(Name = "Amount (£)")]
        [Range(0.1, 10000)]
        public decimal Amount { get; set; }

        public bool Confirmed { get; set; }

        public bool Archived { get; set; }

        public DateTime Created { get; set; }

        public virtual PaymentRequest PaymentRequest { get; set; }
    }
}
