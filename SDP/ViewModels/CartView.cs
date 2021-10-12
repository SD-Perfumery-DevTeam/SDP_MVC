using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.ViewModels
{
    public class CartView
    {
        public Cart cart { get; set; }
        public List<Product> productList { get; set; }
    }
}
