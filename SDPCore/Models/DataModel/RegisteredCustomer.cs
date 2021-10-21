using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;

namespace Microsoft.SDP.SDPCore.Models
{
    public class RegisteredCustomer :  IdentityUser , ICustomer
    {
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }
        public Guid userId { get; set; }

        private IDbRepo _dbRepo;
        public RegisteredCustomer(IDbRepo dbRepo) 
        {
            this.orderList = new List<Order>();
            userId = Guid.NewGuid();
            _dbRepo = dbRepo;
            cart = new Cart();
        }

    }
}
