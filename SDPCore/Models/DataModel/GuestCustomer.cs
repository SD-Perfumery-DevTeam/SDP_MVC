using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    [Serializable]
    public class GuestCustomer : ICustomer
    {
        public Guid userId { get; set; }
        public Cart cart { get; set; }
        public List<Order> orderList { get; set; }
        private IDbRepo _dbRepo;

        public GuestCustomer(IDbRepo dbRepo)
        {
            this.orderList = new List<Order>();
            userId = Guid.NewGuid();
            _dbRepo = dbRepo;
            cart = new Cart();

        }
        public Task payment(string info)
        {
            throw new NotImplementedException();
        }
    }
}
