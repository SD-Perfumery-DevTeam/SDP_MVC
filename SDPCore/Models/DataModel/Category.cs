
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Category
    {
        public Category( string title, string imgUrl, string text)
        {
            this.categoryId = Guid.NewGuid();
            this.title = title;
            this.imgUrl = imgUrl;
            this.text = text;
        }

        public Guid categoryId { get; set; }
        public string title{ get; set; }
        public string imgUrl { get; set; }
        public string text { get; set; }

        public static explicit operator Category(SelectListItem v)
        {
            throw new NotImplementedException();
        }
    }
}
