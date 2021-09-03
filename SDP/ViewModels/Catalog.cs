using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.ViewModels
{
    public class Catalog
    {
        public IEnumerable<Product> products { get; set; }
        public Product product { get; set; }
    }
}
