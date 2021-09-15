using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class Inventory
    {
        public Inventory() 
        { }
        
        public Inventory(Guid inventoryId, Product product, int stockQty)
        {
            this.inventoryId = inventoryId;
            this.product = product;
            this.stockQty = stockQty;
        }

        public Guid inventoryId { get; set; }
        public Product product { get; set; }
        public int stockQty { get; set; }
    }
}
