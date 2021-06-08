using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRSServer.Models
{
    public class User
    {
        public int Id { get; set; } //PK
        [Required, StringLength(30)]
        public string Username { get; set; } //Username is unique, but not a Pk
        [Required, StringLength(30)]
        public string Password { get; set; }
        [Required, StringLength(30)]
        public string Firstname { get; set; }
        [Required, StringLength(30)]
        public string Lastname { get; set; }
        [StringLength(12)]
        public string Phone { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        public bool IsReviewer { get; set; } //bool defaults to false
        [Required]
        public bool IsAdmin { get; set; }

        public User() { } //constructor

    }
}
