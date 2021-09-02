using SDP.Interfaces;
using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Services
{
    public static class ViewService
    {
        public static ICustomer getCustomerFromDB(string id) 
        {
            ICustomer customer =null;
            foreach (ICustomer c in Global.customerList)
            {
                try
                {
                    if (c.userId == Guid.Parse(id))
                    {
                        customer = c;
                    }
                }
                catch (Exception ex)
                {
                    //logging it
                }
            }
            return customer;
        }

        public static Product getProductFromDB(string id) 
        {
            Product product = null;
            foreach (Product p in MockDB.MockProductDB)
            {
                try
                {
                    if (p.productId == Guid.Parse(id))
                    {
                        product = p;
                    }
                }

                catch (Exception ex)
                {
                    //logging it
                }

            }
            return product; 
        }    
    }
}
