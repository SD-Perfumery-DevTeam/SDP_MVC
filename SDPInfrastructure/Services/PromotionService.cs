using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDPInfrastructure.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IDbRepo _dbRepo;

        public PromotionService(IDbRepo dbRepo)
        {
            _dbRepo = dbRepo;
        }

        public string GetPromoProductId(string promoCode) 
        {
           return  _dbRepo.getPromotionBypromoCode(promoCode) == null ? null : _dbRepo.getPromotionBypromoCode(promoCode).product.productId.ToString();
        }

        public bool validatePromo(string promoCode)
        {
            if (_dbRepo.getPromotionBypromoCode(promoCode) == null || (!_dbRepo.getPromotionBypromoCode(promoCode).isActive || _dbRepo.getPromotionBypromoCode(promoCode).startDate > DateTime.UtcNow || _dbRepo.getPromotionBypromoCode(promoCode).endDate < DateTime.UtcNow))
            {
                return false;
            }
            return  true;
        } 
    }
}
