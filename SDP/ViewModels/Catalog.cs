using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Interface;

namespace SDP.ViewModels
{
    public class Catalog
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Brand> brands { get; set; }
        public Product product { get; set; }
        public ICustomer customer { get; set; }
        public Brand brand { get; set; }
        public int totalPage { get; set; }
    }
}
