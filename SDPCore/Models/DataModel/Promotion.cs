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

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Promotion Image")]
        public string imgUrl { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Product")]
        public Product product { get; set; }

        [Required]
        [Display(Name = "Promotion Code")]
        public string promoCode { get; set; }

        [Required]
        [Display(Name = "Discount Percentage")]
        public decimal discount { get; set; }

        [Display(Name = "Start Date")]
        public DateTime startDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool isActive { get; set; }
    }
}
