using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRSServer.Models;

namespace PRSServer.Data
{
    public class PRSServerContext : DbContext
    {
        public PRSServerContext(DbContextOptions<PRSServerContext> options)
            : base(options)
        {
        }

        public DbSet<PRSServer.Models.User> Users { get; set; }
        public DbSet<PRSServer.Models.Vendor> Vendors { get; set; }
        public DbSet<PRSServer.Models.Product> Products { get; set; }
        public DbSet<PRSServer.Models.Request> Requests { get; set; }
        public DbSet<PRSServer.Models.RequestLine> RequestLines { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(u =>
            {
                u.HasIndex(un => un.Username).IsUnique();
            });
            builder.Entity<Vendor>(v =>
            {
                v.HasIndex(c => c.Code).IsUnique();
            });
            builder.Entity<Product>(p =>
            {
                p.HasIndex(pn => pn.PartNbr).IsUnique();
            });

        }
    }
}
