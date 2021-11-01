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
        private static SDPDbContext _db;
        private static UserManager<IdentityUser> _userManager;

        public Task<IdentityUser> getCustomerAsync(string Id);

        public Article GetArticle(string Id);
        public Brand GetBrand(string Id);
        public Category GetCategory(string Id);
        public int GetInventory(string Id);
        public Product getProduct(string Id);
        public IEnumerable<Product> GetProductList();
        public Promotion getPromotionBypromoCode(string promoCode);
        public Promotion getPromotionByID(string promoId);
        public IEnumerable<IdentityUser> GetCustomerList();
    }
}
