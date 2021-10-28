using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Interface;
using SDPCore.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.SDP.SDPCore.Consts;

namespace Microsoft.SDP.SDPCore.Models
{
    //cart object is not present in the database
    //cart list stores the product id as a key and the quantity of said prodcut and any discount as value
    public class Cart
    {
        public int cartID { get; set; }
        public bool promotionActive { get; set; } = false;
        public Dictionary<string, CartValuePair> cartList = new Dictionary<string, CartValuePair>(); 

        public Dictionary<string, CartValuePair> getCartList() { return cartList; }

        public void addProductToCart(Product product, int count)  
        {
            if (cartList.ContainsKey(product.productId.ToString()))
            {
                cartList[product.productId.ToString()].quantity = cartList[product.productId.ToString()].quantity + count;
            }
            else cartList.Add(product.productId.ToString(), new CartValuePair { quantity =count, discount = 0 });
        }

        public void removeProductToCart(string productId)
        {
            if (cartList.ContainsKey(productId))
            {
                cartList.Remove(productId);
            }
        }

        public OrderDataTransfer turnCartToOrder(int orderNo, IdentityUser user, Delivery delivery, decimal totalPrice, string paymentStatus, DateTime paymentDate, OrderStatus orderStatus, List<Product> productList ) //turns the cart into order and orderlines using dto
        {
            Order order = new Order(orderNo, user, delivery, totalPrice, paymentStatus, paymentDate, orderStatus);
            List<OrderLine> orderLineList = new List<OrderLine>();
            foreach (var pair in cartList)
            {
                Product p = productList.Where(m => m.productId.ToString() == pair.Key).FirstOrDefault();
                orderLineList.Add(new OrderLine (order, p, pair.Value.quantity, pair.Value.discount));
            }
            order.orderLine = orderLineList;
            return new OrderDataTransfer { order = order, orderLineList = orderLineList };
        }

        public class CartValuePair //internal class recording both quantity and discount
        {
            public int quantity { get; set; }
            public decimal discount { get; set; }
        }
    }
}




