using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Order
    {
        public Guid ID { get; set; }
        public ICustomer customer{ get; set; }
        public List<OrderLine> OLList { get; set; }//OrderLine list 
        public Delivery delivery;

        public Order(ICustomer customer)
        {
            ID = Guid.NewGuid();
            this.customer = customer;
        }
    }
}
