using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SDP.ViewModels
{
    public class AddProduct
    {
        public IEnumerable<SelectListItem> categories { get; set; }
        public IEnumerable<SelectListItem> brands { get; set; }
        public IEnumerable<SelectListItem> inventories { get; set; }
        public Inventory inventory { get; set; }
        public Product product { get; set; }
        public IFormFile Image { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
