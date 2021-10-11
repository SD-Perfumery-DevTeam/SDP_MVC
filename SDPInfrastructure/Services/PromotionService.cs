﻿using Microsoft.SDP.SDPCore.Interface;
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

        public string validatePromoCode(string promoCode) 
        {
           return  _dbRepo.getPromotion(promoCode).product.productId.ToString();
        }

        public bool validatePromoDate(string promoCode)
        {
            if (_dbRepo.getPromotion(promoCode).startDate > DateTime.UtcNow || _dbRepo.getPromotion(promoCode).endDate < DateTime.UtcNow)
            {
                return false;
            }
            return  true;
        } 

    }
}