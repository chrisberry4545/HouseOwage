using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseOwage.Models
{
    public class PaymentRequest
    {
        public int PaymentRequestId { get; set; }

        [Display(Name = "Amount (£)")]
        [Range(0.1, 10000)]
        public decimal Amount { get; set; }
        public String Name { get; set; }

        public virtual User CreatedBy { get; set; }

        public bool Archived { get; set; }

        [ForeignKey("SentTo")]
        public int SentTo_UserId { get; set; }
        [Display(Name = "Send to")]
        public virtual User SentTo { get; set; }

        public virtual IEnumerable<Payment> Payments { get; set; }
    }
}
