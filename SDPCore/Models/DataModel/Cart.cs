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
    // cart object is not present in the database
    // cart list stores the product id as a key and the quantity of said prodcut and any discount as value
    public class Cart
    {
        public int cartID { get; set; }
        public bool promotionActive { get; set; } = false;
        public Dictionary<string, CartValuePair> cartList = new Dictionary<string, CartValuePair>(); 

        public Dictionary<string, CartValuePair> getCartList() { return cartList; }

        // adding a product to the cart
        public void addProductToCart(Product product, int count)  
        {
            // if the same product is added to the cart again, append the quantity
            if (cartList.ContainsKey(product.productId.ToString()))
            {
                cartList[product.productId.ToString()].quantity = cartList[product.productId.ToString()].quantity + count;
            }
            else cartList.Add(product.productId.ToString(), new CartValuePair { quantity =count, discount = 0 });
        }

        // removing a product from the cart
        public string removeProductToCart(string productId)
        {
            if (cartList.ContainsKey(productId))
            {
                cartList.Remove(productId);
                return "deleted";
            }
            return "nothing was deleted";
        }

        // placing the cart into an order
        public OrderDataTransfer turnCartToOrder(int orderNo, IdentityUser user, Delivery delivery, decimal totalPrice, string paymentStatus, DateTime paymentDate, OrderStatus orderStatus, List<Product> productList ) //turns the cart into order and orderlines using dto
        {
            // create order and orderline
            Order order = new Order(orderNo, user, delivery, totalPrice, paymentStatus, paymentDate, orderStatus);
            List<OrderLine> orderLineList = new List<OrderLine>();

            // for each product in the cart
            foreach (var pair in cartList)
            {
                // get the product object
                Product p = productList.Where(m => m.productId.ToString() == pair.Key).FirstOrDefault();

                // add the product to the order line
                orderLineList.Add(new OrderLine (order, p, pair.Value.quantity, pair.Value.discount));
            }

            // with all items added to the order line, tie the order line to the order
            order.orderLine = orderLineList;

            // 
            return new OrderDataTransfer { order = order, orderLineList = orderLineList };
        }

        // inner class that stores each cart product's quantity and discount
        public class CartValuePair
        {
            public int quantity { get; set; }
            public decimal discount { get; set; }
        }
    }
}




