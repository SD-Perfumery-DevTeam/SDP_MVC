using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SDPCore.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models.DbContexts
{
    public class SDPDbContext:IdentityDbContext
    {
        public DbSet<Article> article { get; set; }
        public DbSet<Brand> brand { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<Delivery> delivery { get; set; }
        public DbSet<Inventory> inventory{ get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderLine> orderLine { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Promotion> promotion { get; set; }
        public DbSet<UserSettings> userSettings { get; set; }
        /*  
          
          public DbSet<RegisteredCustomer> registeredCustomers { get; set; }
        */

        public SDPDbContext(DbContextOptions<SDPDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
