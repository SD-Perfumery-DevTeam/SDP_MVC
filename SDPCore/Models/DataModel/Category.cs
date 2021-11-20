using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Category
    {
        public Guid categoryId { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Title")]
        public string title{ get; set; }

        [StringLength(2048)]
        [Display(Name = "Category Image")]
        public string imgUrl { get; set; }

        [StringLength(2048)]
        [Display(Name = "Title")]
        public string text { get; set; }

        // Default constructor
        public Category() { }

        // Constructor without specifying a categoryId
        public Category( string title, string imgUrl, string text)
        {
            this.categoryId = categoryId;
            this.title = title;
            this.imgUrl = imgUrl;
            this.text = text;
        }

        // Constructor specifying a categoryId
        public Category( Guid categoryId, string title, string imgUrl, string text)
        {
            this.categoryId = categoryId;
            this.title = title;
            this.imgUrl = imgUrl;
            this.text = text;
        }

        public static explicit operator Category(SelectListItem v)
        {
            throw new NotImplementedException();
        }
    }
}
