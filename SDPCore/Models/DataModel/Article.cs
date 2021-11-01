using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Article
    {
        public Guid articleId { get; set; }
        public string title { get; set; }
        public string imgUrl { get; set; }
        public string text { get; set; }
    }
}
