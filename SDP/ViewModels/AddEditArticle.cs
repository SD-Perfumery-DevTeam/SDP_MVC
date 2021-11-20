using Microsoft.AspNetCore.Http;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.ViewModels
{
    public class AddEditArticle
    {
        public Article article { get; set; }
        public IFormFile image { get; set; }

        // Only used for viewing all articles.
        public IEnumerable<Article> articleList { get; set; }
    }
}
