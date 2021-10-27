using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Interface
{
    public interface IDbRepo
    {
        public Product getProduct(string Id);
        public Task<IdentityUser> getCustomerAsync(string Id);
        private static SDPDbContext _db;
        private static UserManager<IdentityUser> _userManager;
        public Promotion getPromotionBypromoCode(string promoCode);
        public Promotion getPromotionByID(string promoId);
        public IEnumerable<Product> GetProductList();
        public IEnumerable<IdentityUser> GetCustomerList();
        public int GetInventory(string Id);
    }
}
