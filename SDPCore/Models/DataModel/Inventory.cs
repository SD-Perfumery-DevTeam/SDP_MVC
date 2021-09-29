using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
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

        [Display(Name = "Quantity in Stock")]
        public int stockQty { get; set; }
    }
}
