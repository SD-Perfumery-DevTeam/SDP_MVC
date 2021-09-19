using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class Promotion
    {
        public Promotion() 
        { }

        public Promotion(Guid promoId, string title, string imgUrl, 
            string description, Product product, string promoCode, 
            decimal discount, DateTime statDate, DateTime endDate)
        {
            this.promoId = promoId;
            this.title = title;
            this.imgUrl = imgUrl;
            this.description = description;
            this.product = product;
            this.promoCode = promoCode;
            this.discount = discount;
            this.statDate = statDate;
            this.endDate = endDate;
        }

        public Guid promoId { get; set; }
        public string title { get; set; }
        public string imgUrl { get; set; }
        public string description { get; set; }
        public Product product { get; set; }
        public string promoCode { get; set; }
        public decimal discount { get; set; }
        public DateTime statDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
