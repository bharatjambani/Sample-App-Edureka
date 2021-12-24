using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Models
{
    //public class BankOfAmericaContext:DbContext
    public class BankOfAmericaContext : IdentityDbContext<ApplicationUser>
    {
        public BankOfAmericaContext(DbContextOptions<BankOfAmericaContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

    }
}
