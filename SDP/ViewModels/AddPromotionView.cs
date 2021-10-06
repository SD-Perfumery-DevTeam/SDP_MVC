using Microsoft.AspNetCore.Http;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.ViewModels
{
    public class AddPromotionView
    {
        public Promotion promotion { get; set; }
        public IFormFile Image { get; set; }

        //used for viewing all promotions
        public IEnumerable<Promotion> promotionList{ get; set; }
    }
}
