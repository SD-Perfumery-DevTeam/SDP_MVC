using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Article
    {
        public Guid articleId { get; set; }

        private string _key;

        // Define a human readable key string that can be used to access an
        // article via typed URL.
        // Note: this is dynamically generated and not a database object.
        // Reference: https://www.linkedin.com/learning/learning-asp-dot-net-core-mvc/
        [NotMapped]
        public string key
        {
            get
            {
                if (_key == null)
                {
                    _key = Regex.Replace(title.ToLower(), "[^a-z0-9]", "-");
                }
                return _key;
            }
            set
            {
                _key = value;
            }
        }

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
