using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Interface
{
    public interface ICustomer
    {
        Guid userId { get; set; }
        Cart cart { get; set; }
        List<Order> orderList { get; set; }
    }
}
