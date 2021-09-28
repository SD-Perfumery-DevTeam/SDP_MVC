using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class Inventory
    {
        // Default Constructor
        public Inventory() { }

        // Constructor that provides a new Guid
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
