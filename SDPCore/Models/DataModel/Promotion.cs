using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Promotion
    {
        public Promotion()
        { }

        public Promotion(Guid promoId, string title, string imgUrl,
            string description, Product product, string promoCode,
            decimal discount, DateTime startDate, DateTime endDate,
            bool isActive)
        {
            this.promoId = promoId;
            this.title = title;
            this.imgUrl = imgUrl;
            this.description = description;
            this.product = product;
            this.promoCode = promoCode;
            this.discount = discount;
            this.startDate = startDate;
            this.endDate = endDate;
            this.isActive = isActive;
        }
        [Key]
        public Guid promoId { get; set; }
        public string title { get; set; }
        public string imgUrl { get; set; }
        public string description { get; set; }
        [Required]
        public Product product { get; set; }
        [Required]
        public string promoCode { get; set; }
        [Required]
        public decimal discount { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        [Required]
        public bool isActive { get; set; } 
    }
}
