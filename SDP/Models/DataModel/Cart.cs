using Newtonsoft.Json;
using SDP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    
    public class Cart
    {
        public int cartID { get; set; }

       public Dictionary<Product, int> cartList = new Dictionary<Product, int>();

       public void addOrderToCustomerList(Order order) => order.customer.orderList.Add(order);
   
        
        public Dictionary<Product, int> getCartList() { return cartList; }
        public void addProductToCart(Product product, int count)  //adding the 
        {
            if (cartList.ContainsKey(product))
            {
                cartList[product] = cartList[product] + count;
            }
            else cartList.Add(product, count);
        }




   /*     public async Task checkOut()
        {
            await customer.payment("bank info");
            *//* turnCartToOrder().delivery = new Delivery("DATA BROWSER NEEDED HERE");*//*
        }*/
    }
}




