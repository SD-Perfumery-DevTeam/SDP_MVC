using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class Brand
    {   
        //default constructor for EF
        public Brand() { }
        public Brand(Guid brandId, string title)
        {
            this.brandId = brandId;
            this.title = title;
        }

        public Guid brandId { get; set; }
        public string  title { get; set; }
    }
}
