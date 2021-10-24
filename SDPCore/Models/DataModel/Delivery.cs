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
        public string deliveryName { get; set; }
        public string email { get; set; }
        public int phone { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string postCode { get; set; }
        public string suburb { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public DeliveryStatus deliverystatus { get; set; }
        public DateTime deliveryDate { get; set; }

        public Delivery()
        { 
        }

        public Delivery( string email, int phone, string addressL1, string addressL2, string postCode, string suburb, string state, string country, DeliveryStatus deliverystatus, DateTime deliveryDate)
        {
            this.deliveryId = Guid.NewGuid();
            this.email = email;
            this.phone = phone;
            this.addressLine1  = addressL1;
            this.addressLine2  = addressL2;
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
