using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SDP.Consts;

namespace SDP.Models
{
    public class Delivery
    {
        public Guid ID;

        public Delivery(Order order, string email, int phone, string addressL1,
            string addressL2, int postCode, Status status)
        { 
            ID = Guid.NewGuid();
            this.email = email;
            this.phone = phone;
            this.addressL1 = addressL1;
            this.addressL2 = addressL2;
            this.postCode = postCode;
            this.status = status;
            this.order = order;
        }

        public Order order { get; set; }
        public string email { get; set; }
        public int phone { get; set; }
        public string addressL1 { get; set; }
        public string addressL2 { get; set; }
        public int postCode { get; set; }
        public Status status { get; set; }

        public void changeStatus(Status s) => status = s;
        
    }
}
