using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContext;
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

        private static ProductDbContext _db;
        private static UserManager<IdentityUser> _userManager;
        public Promotion getPromotion(string promoCode);
    }
}
