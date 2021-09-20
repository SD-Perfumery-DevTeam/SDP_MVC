using Microsoft.EntityFrameworkCore;
using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models.DbContext
{
    public class DbRepo : IDbRepo
    {
        
        private static ProductDbContext _db;


        public DbRepo(ProductDbContext db)
        {
            _db = db;
        }

        public Product getProduct(string Id) 
        {
            return  _db.product.Include(m => m.brand).Include(m => m.category).ToList().Find(m => m.productId == Guid.Parse(Id));
        }

    }
}
