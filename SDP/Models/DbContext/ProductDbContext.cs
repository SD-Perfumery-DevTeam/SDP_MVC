using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models.DbContext
{
    public class ProductDbContext:IdentityDbContext
    {
        public DbSet<Product> products { get; set; }
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
