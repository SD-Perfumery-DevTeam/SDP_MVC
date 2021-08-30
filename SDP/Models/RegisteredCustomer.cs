using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class RegisteredCustomer :  Customer, IUser
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        
        public RegisteredCustomer() 
        {
            this.orderList = new List<Order>();
            ID = Guid.NewGuid();
            cart = new Cart(this);
        }
    }
}
