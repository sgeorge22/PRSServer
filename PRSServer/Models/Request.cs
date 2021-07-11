using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PRSServer.Models
{
    public class Request
    {
        //Created the below for application updates 
        //public static string StatusNew { get; set; } = "NEW";
        //public static string StatusEdit { get; set; } = "EDIT";
        //public static string StatusReview { get; set; } = "REVIEW";
        //public static string StatusApprove { get; set; } = "APPROVED";
        //public static string StatusRejected { get; set; } = "REJECTED";

        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Description { get; set; }
        [Required, StringLength(80)]
        public string Justification { get; set; }
        [StringLength(80)]
        public string RejectionReason { get; set; }
        [Required, StringLength(20)]
        public string DeliveryMode { get; set; } = "Pickup";
        [Required]
        public string Status { get; set; } = "NEW"; 
        [Required, Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; } = 0; //grand total off all items in the order, defults to zero
        [Required]
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual List<RequestLine> RequestLines { get; set; }

        public Request() { }

    }
}
