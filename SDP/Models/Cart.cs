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
        private List<Product> cartList = new List<Product>();
        public Cart(ICustomer customer) => this.customer =customer;

        public ICustomer customer;

        

     

        private void addOrderToCustomerList(Order order) => order.customer.orderList.Add(order);
        public void addToCart(Product p) => cartList.Add(p);
        public List<Product> getCartList() { return cartList; }
        

        //turns the cartLIst into an Order obj with orderline embeded
        public Order turnCartToOrder()
        {
            if(cartList.Any())
            {
                Order o = new Order(this.customer);
                foreach (Product p in cartList)
                {
                    foreach (OrderLine ol in o.OLList)
                    {
                        if (ol.getProduct().Equals(p))
                        {
                            ol.quantity++;
                        }
                    }
                    OrderLine temp = new OrderLine(o, p, 1, null);
                    o.OLList.Add(temp);
                }
                addOrderToCustomerList(o);
                return o;
            }
            return null;
        }
      
        public async Task checkOut() 
        {
            await customer.payment("bank info");
            /* turnCartToOrder().delivery = new Delivery("DATA BROWSER NEEDED HERE");*/
        }
        //used for mock db
        public HashSet<Product> getProductListWithCount()
        {
            //note that the product list is static so the count of the product would be global... for now
            List<Product> refCartList = new List<Product>();

            //deep clone
            refCartList = cartList.ConvertAll(product => new Product(product.productId, product.title,
                product.price, product.onSpecial, product.imgUrl, product.productType, product.packageQty,
                product.brand, product.description, product.count));
            HashSet<Product> unique = new HashSet<Product>();
            foreach (Product p in cartList)
            {
                if (unique.Contains(p))
                {
                    p.count++;
                }
                else unique.Add(p);
            }
            return unique;
        }

    }
}




