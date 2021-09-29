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
        private Order order { get; set; }
        private Product product { get; set; }
        public int quantity { get; set; }
        public IPromotion promotion;

        public OrderLine(Order order, Product product, 
            int quantity, IPromotion promotion)
        {

            lineId = Guid.NewGuid();
            this.order = order;
            this.product = product;
            this.quantity = quantity;
            this.promotion = promotion;
        }

        public Order getOrder() => this.order;
        public Product getProduct() => this.product;
    }
}
