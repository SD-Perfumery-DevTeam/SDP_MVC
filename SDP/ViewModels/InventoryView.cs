using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.ViewModels
{
    public class InventoryView
    {
        public IEnumerable<Inventory> inventories { get; set; }
        public Inventory inventory { get; set; }
       
    }
}
