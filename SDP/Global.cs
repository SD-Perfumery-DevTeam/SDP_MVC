using Microsoft.AspNetCore.Http;
using SDP.Interfaces;
using SDP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDP
{
    public static class Global
    {
        private static List<Product> P_list = new List<Product>();
        public static List<ICustomer> customerList = new List<ICustomer>();


        public static async Task<bool> UploadFile(IFormFile ufile)
        {
            
            return false;
        }
    }
}
