using Microsoft.SDP.SDPCore.Models;
using SDPCore.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.ViewModels
{
    public class CheckoutView
    {
        public decimal amount { get; set; }
        public Key key { get; set; }
        public Delivery delivery { get; set; }
    }
}
