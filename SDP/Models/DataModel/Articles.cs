using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public class Articles
    {
        public Guid Id { get; set; }
        public string ArticleTitle { get; set; }
        public DateTime Date { get; set; }
        public string ArticleBody { get; set; }
    }
}
