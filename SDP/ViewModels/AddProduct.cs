﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SDP.Models;
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
        public Product product { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
