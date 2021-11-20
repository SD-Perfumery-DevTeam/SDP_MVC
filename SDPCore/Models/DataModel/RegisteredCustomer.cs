using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;

namespace Microsoft.SDP.SDPCore.Models
{
    public class RegisteredCustomer : IdentityUser , ICustomer
    {
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }
        

        private IDbRepo _dbRepo;
        public RegisteredCustomer(IDbRepo dbRepo) 
        {
            this.orderList = new List<Order>();
            Id = Guid.NewGuid().ToString();
            _dbRepo = dbRepo;
            cart = new Cart();
        }
    }
}
