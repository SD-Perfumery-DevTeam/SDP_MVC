using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SDP.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models.DbContext
{
    public class ProductDbContext:IdentityDbContext
    {
        public DbSet<Product> product { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<Brand> brand { get; set; }
        public DbSet<Inventory> inventory{ get; set; }

        /*  
         public DbSet<Tag> tag { get; set; }
          public DbSet<Order> orders { get; set; }
          public DbSet<Customer> customers { get; set; }
          public DbSet<RegisteredCustomer> registeredCustomers { get; set; }
          public DbSet<OrderLine> orderLines { get; set; }
          public DbSet<Articles> articles { get; set; }
          public DbSet<Delivery> deliveries { get; set; }*/

        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
