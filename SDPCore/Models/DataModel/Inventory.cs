using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    // Inventory is a back-end instance of a product, containing a stock quantity level
    public class Inventory
    {
        // Default Constructor
        public Inventory() { }

        // Constructor for a new Inventory that provides a new Guid
        public Inventory(Product product, int stockQty)
        {
            this.inventoryId = Guid.NewGuid();
            this.product = product;
            this.stockQty = stockQty;
        }

        // Constructor that takes an existing Guid
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
