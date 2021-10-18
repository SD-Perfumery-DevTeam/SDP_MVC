using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class RegisteredCustomer : User, ICustomer
    {
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }
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
