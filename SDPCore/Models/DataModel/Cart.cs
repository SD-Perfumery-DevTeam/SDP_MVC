﻿using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    //cart object is not present in the database
    //cart list stores the product id as a key and the quantity of said prodcut and any discount as value
    public class Cart
    {
        public int cartID { get; set; }
        public bool promotionActive { get; set; } = false;
        public Dictionary<string, CartValuePair> cartList = new Dictionary<string, CartValuePair>(); 

        public void addOrderToCustomerList(Order order) => order.customer.orderList.Add(order);
        
        
        public Dictionary<string, CartValuePair> getCartList() { return cartList; }

        public void addProductToCart(Product product, int count)  
        {
            if (cartList.ContainsKey(product.productId.ToString()))
            {
                cartList[product.productId.ToString()].quantity = cartList[product.productId.ToString()].quantity + count;
            }
            else cartList.Add(product.productId.ToString(), new CartValuePair { quantity =count, discount = 0 });
        }
        public class CartValuePair //internal class recording both quantity and discount
        {
            public int quantity { get; set; }
            public decimal discount { get; set; }
        }
    }

}



/*     public async Task checkOut()
             {
                 await customer.payment("bank info");
                 *//* turnCartToOrder().delivery = new Delivery("DATA BROWSER NEEDED HERE");*//*
             }*/
