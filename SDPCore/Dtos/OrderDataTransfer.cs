using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDPCore.Dtos
{
    public class OrderDataTransfer
    {
        public Order order { get; set; }
        public List<OrderLine> orderLineList { get; set; }
    }
}
