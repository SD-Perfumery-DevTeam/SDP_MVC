using Microsoft.AspNetCore.Http;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Interface;

namespace SDP
{
    public static class GlobalVar
    {
        private static List<Product> P_list = new List<Product>();
        public static List<ICustomer> customerList = new List<ICustomer>();
    }
}
