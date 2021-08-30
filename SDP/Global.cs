using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP
{
    public static class Global
    {
        private static List<Product> P_list = new List<Product>();
        public static List<Customer> customerList = new List<Customer>();
        public static bool addProduct(Product p) 
        {
            foreach (Product existingProduct in P_list) 
            {
                if (existingProduct == p) 
                {
                    return false;
                }
            }
            P_list.Add(p);
            return true;
        }

        public static bool rmProduct(Product p)
        {
            foreach (Product existingProduct in P_list)
            {
                if (existingProduct == p)
                {
                    P_list.Remove(p);
                    return true;
                }
            }
            
            return false;
        }
    }
}
