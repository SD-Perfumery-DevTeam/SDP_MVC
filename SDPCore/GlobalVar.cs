using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore
{
    public static class GlobalVar
    {
        private static List<Product> P_list = new List<Product>();
        public static List<ICustomer> customerList = new List<ICustomer>();
    }
}
