using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPInfrastructure.Services
{
    public class GuestCustomerService : ICustomer 
    {
        public Guid userId { get; set; }
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }
        public GuestCustomerService()
        {
            this.orderList = new List<Order>();
            userId = Guid.NewGuid();
            cart = new Cart();
        }
        public Task payment(string info)
        {
            throw new NotImplementedException();
        }
        //turns the cartLIst into an Order obj with orderline embeded
        public Order turnCartToOrder()
        {
            if (cart.cartList.Any())
            {
                Order o = new Order(this);
                foreach (var p in cart.cartList)
                {
                    foreach (OrderLine ol in o.OLList)
                    {
                        if (ol.getProduct().Equals(p))
                        {
                            ol.quantity++;
                        }
                    }
                    OrderLine temp = new OrderLine(o, p.Key, 1, null);
                    o.OLList.Add(temp);
                }
                cart.addOrderToCustomerList(o);
                return o;
            }
            return null;
        }
    }
}
