using Microsoft.SDP.SDPCore;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.ViewModels
{
    public class OrderView
    {
        public Order order { get; set; }
        public List<OrderLine> orderLineList { get; set; }
        public Consts.OrderStatus orderStatus { get; set; }
        public Consts.DeliveryStatus deliveryStatus { get; set; }
    }
}
