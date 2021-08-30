using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    public abstract class Category
    {
        public Guid ID { get; set; }
        public Consts.CateTypes categoryType { get; set; }
    }
}
