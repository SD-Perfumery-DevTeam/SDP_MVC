﻿using SDP.Interfaces;
using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Services
{
    public static class ViewService
    {
        public static ICustomer getCustomerFromList(string id) 
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
        public static void DeleteCustomerFromList(string id)
        {
            ICustomer customer = null;
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
            if (customer != null)
            {
                Global.customerList.Remove(customer);
            }
        }

    }
}
