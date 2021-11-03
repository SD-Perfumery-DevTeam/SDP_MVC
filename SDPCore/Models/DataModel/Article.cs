using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Article
    {
        public Guid articleId { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Title")]
        public string title { get; set; }

        [StringLength(2048)]
        [Display(Name = "Article Image")]
        public string imgUrl { get; set; }

        [Required]
        [Display(Name = "Article Text")]
        public string text { get; set; }

        // Default Constructor
        public Article() { }

        // Constructor without specifying an articleId
        public Article(string title, string imgUrl, string text)
        {
            this.articleId = articleId;
            this.title = title;
            this.imgUrl = imgUrl;
            this.text = text;
        }

        // Constructor specifying an articleId
        public Article(Guid articleId, string title, string imgUrl, string text)
        {
            this.articleId = articleId;
            this.title = title;
            this.imgUrl = imgUrl;
            this.text = text;
        }
    }
}
