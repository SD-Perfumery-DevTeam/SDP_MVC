using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Brand
    {   
        //default constructor for EF
        public Brand() { }
        public Brand( string title)
        {
            this.brandId = brandId;
            this.title = title;
        }
        public Brand(Guid brandID, string title)
        {
            this.brandId = brandID;
            this.title = title;
        }
        public Guid brandId { get; set; }
        public string  title { get; set; }
    }
}
