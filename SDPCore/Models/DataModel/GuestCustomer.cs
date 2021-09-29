﻿using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
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
