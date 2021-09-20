using SDP.Models;
using SDP.Models.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Interfaces
{
    public interface IDbRepo
    {
        public Product getProduct(string Id);
        private static ProductDbContext _db;
    }
}
