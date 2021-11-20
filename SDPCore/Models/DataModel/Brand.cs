using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Brand
    {   
        public Guid brandId { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Title")]
        public string  title { get; set; }

        // Default constructor
        public Brand() { }

        // Constructor without specifying a brandId
        public Brand( string title)
        {
            this.brandId = brandId;
            this.title = title;
        }

        // Constructor specifying a brandId
        public Brand(Guid brandID, string title)
        {
            this.brandId = brandID;
            this.title = title;
        }
    }
}
