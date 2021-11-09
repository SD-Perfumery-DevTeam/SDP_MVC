using Microsoft.AspNetCore.Http;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.ViewModels
{
    public class AddEditCategory
    {
        public Category category { get; set; }
        public IFormFile image { get; set; }
    }
}
