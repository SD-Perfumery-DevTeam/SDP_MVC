using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models.DbContexts
{
    public class DbRepo : IDbRepo 
    {
        
        private  SDPDbContext _db;
        private  UserManager<IdentityUser> _userManager { get; }

        public DbRepo(SDPDbContext db, UserManager<IdentityUser> um)
        {
            _db = db;
            _userManager = um;
        }

        public async Task<IdentityUser> getCustomerAsync(string Id)
        {
            return await _userManager.FindByIdAsync(Id) ;
        }

        public Article GetArticle(string Id)
        {
            return !string.IsNullOrWhiteSpace(Id) ? _db.article.ToList().Find(m => m.articleId == Guid.Parse(Id)) : null;
        }

        public Brand GetBrand(string Id)
        {
            return !string.IsNullOrWhiteSpace(Id) ? _db.brand.ToList().Find(m => m.brandId == Guid.Parse(Id)) : null;
        }

        public Category GetCategory(string Id)
        {
            return !string.IsNullOrWhiteSpace(Id) ? _db.category.ToList().Find(m => m.categoryId == Guid.Parse(Id)) : null;
        }
        
        public int GetInventory(string Id)
        {
            foreach (var inventory in _db.inventory.ToList())
            {
                if (inventory.product.productId.ToString() == Id)
                {
                    return inventory.stockQty;
                }
            }
            return 0;
        }

        public Product getProduct(string Id) 
        {
            return !string.IsNullOrWhiteSpace(Id)? _db.product.Include(m => m.brand).Include(m => m.category).ToList().Find(m => m.productId == Guid.Parse(Id)) : null;
        }

        public IEnumerable<Product> GetProductList() 
        {
            return _db.product.Include(m => m.brand).Include(m => m.category).ToList();
        }

        public Promotion getPromotionBypromoCode(string promoCode)
        {
            return !string.IsNullOrWhiteSpace(promoCode) ?  _db.promotion.Include(m => m.product).ToList().Find(m => m.promoCode == promoCode) : null;
        }

        public Promotion getPromotionByID(string promoId)
        {
            return !string.IsNullOrWhiteSpace(promoId) ? _db.promotion.Include(m => m.product).ToList().Find(m => m.promoId.ToString()== promoId) : null;
        }

        public IEnumerable<IdentityUser> GetCustomerList()
        {
            return _userManager.Users.ToList();
        }
    }
}
