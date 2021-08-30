using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    [Serializable]
    public class GuestCustomer : Customer
    {
        public GuestCustomer()
        {
            this.orderList = new List<Order>();
            ID = Guid.NewGuid();
            cart = new Cart(this);
        }

    }
}
