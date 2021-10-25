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
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
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

        public Delivery( string firstName, string lastName, string email, string phone, string addressLine1, string addressLine2, string postCode, string suburb, string state, string country, DeliveryStatus deliverystatus, DateTime deliveryDate)
        {
            this.deliveryId = Guid.NewGuid();
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phone = phone;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
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
