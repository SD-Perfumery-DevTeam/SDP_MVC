using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class OrderLine
    {
        [Key]
        public Guid lineId { get; set; }
        public Order order { get; set; }
        public Product product { get; set; }
        public decimal quantity { get; set; }
        public decimal discount { get; set; }

        public OrderLine()
        {
        }

        public OrderLine( Order order, Product product, decimal quantity, decimal discount)
        {
            this.lineId = Guid.NewGuid();
            this.order = order;
            this.product = product;
            this.quantity = quantity;
            this.discount = discount;
        }

        public Order getOrder() => this.order;
        public Product getProduct() => this.product;
    }
}
