using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.SDP.SDPCore.Consts;


namespace Microsoft.SDP.SDPCore.Models
{
    public class Delivery
    {

        public Guid deliveryId { get; set; }
        public string email { get; set; }
        public int phone { get; set; }
        public string addressL1 { get; set; }
        public string addressL2 { get; set; }
        public string postCode { get; set; }
        public string suburb { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string deliverystatus { get; set; }
        public DateTime deliveryDate { get; set; }

        public Delivery()
        { 
           
        }

        public Delivery( string email, int phone, string addressL1, string addressL2, string postCode, string suburb, string state, string country, string deliverystatus, DateTime deliveryDate)
        {
            this.deliveryId = Guid.NewGuid();
            this.email = email;
            this.phone = phone;
            this.addressL1 = addressL1;
            this.addressL2 = addressL2;
            this.postCode = postCode;
            this.suburb = suburb;
            this.state = state;
            this.country = country;
            this.deliverystatus = deliverystatus;
            this.deliveryDate = deliveryDate;
        }

        /*public void changeStatus(Status s) => status = s;*/
    }
}
