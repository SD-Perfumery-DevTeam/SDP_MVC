﻿using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPInfrastructure.Services
{
    public static class ViewService
    {
        public static ICustomer getCustomerFromList(string id) 
        {
            ICustomer customer =null;
            var list = GlobalVar.customerList;
            foreach (ICustomer c in GlobalVar.customerList)
            {
                try
                {
                    if (c.Id ==id)
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

        public static IdentityUser getCustomerIdentityFromList(string id)
        {
            IdentityUser customer = null;
            var list = GlobalVar.customerList;
            foreach (ICustomer c in GlobalVar.customerList)
            {
                try
                {
                    if (c.Id == id)
                    {
                        customer = (RegisteredCustomer)c;
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
            foreach (ICustomer c in GlobalVar.customerList)
            {
                try
                {
                    if (c.Id == id)
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
                GlobalVar.customerList.Remove(customer);
            }
        }

    }
}
