using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models.DbContext
{
    public class DbRepo : IDbRepo 
    {
        
        private  ProductDbContext _db;
        private  UserManager<IdentityUser> _userManager { get; }

        public DbRepo(ProductDbContext db, UserManager<IdentityUser> um)
        {
            _db = db;
            _userManager = um;
        }

        public async Task<IdentityUser> getCustomerAsync(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }

        public Product getProduct(string Id) 
        {
            return !string.IsNullOrWhiteSpace(Id)? _db.product.Include(m => m.brand).Include(m => m.category).ToList().Find(m => m.productId == Guid.Parse(Id)) : null;
        }

        public Promotion getPromotion(string promoCode)
        {
            return !string.IsNullOrWhiteSpace(promoCode) ?  _db.promotion.Include(m => m.product).ToList().Find(m => m.promoCode == promoCode) : null;
        }

    }
}
