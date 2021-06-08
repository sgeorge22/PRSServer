using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PRSServer.Models
{
    public class Product
    {
        public int Id { get; set; } //PK
        [Required, StringLength(30)]
        public string PartNbr { get; set; } //PartNbr is unique
        [Required, StringLength(30)]
        public string Name { get; set; }
        [Required, Column(TypeName = "decimal(11,2)")]//column typename is communicating to sql that price is a decimal
        public decimal Price { get; set; }
        [Required, StringLength(30)]
        public string Unit { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }
        [Required]
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; } //creating a connection from a fk to a pk

        public Product() { }

    }
}
