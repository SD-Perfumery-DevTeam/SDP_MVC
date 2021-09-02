using SDP.Interfaces;
using SDP.Models.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class RegisteredCustomer : User, ICustomer
    {
        
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }

        public RegisteredCustomer() 
        {
            this.orderList = new List<Order>();
            userId = Guid.NewGuid();
            cart = new Cart(this);
        }

        public Task payment(string info)
        {
            throw new NotImplementedException();
        }
    }
}
