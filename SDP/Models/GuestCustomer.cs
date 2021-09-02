using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    [Serializable]
    public class GuestCustomer : ICustomer
    {
        public Guid userId { get; set; }
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }
        public GuestCustomer()
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
