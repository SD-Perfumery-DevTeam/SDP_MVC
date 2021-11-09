using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.SDP.SDPCore.Consts;


namespace Microsoft.SDP.SDPCore.Models
{
    public class Delivery
    {
        [Required]
        public Guid deliveryId { get; set; }

        [Display(Name = "First Name")]
        [StringLength(256, ErrorMessage = "Please enter a first name with {0} characters or less.")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(256, ErrorMessage = "Please enter a last name with {0} characters or less.")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(256, ErrorMessage = "Please enter an email address with {0} characters or less.")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(16, ErrorMessage = "Please enter a phone number with {0} digits or less.")]
        public string phone { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(256, ErrorMessage = "Please enter {0} characters or less for address line 1.")]
        public string addressLine1 { get; set; }

        [StringLength(256, ErrorMessage = "Please enter {0} characters or less for address line 2.")]
        public string addressLine2 { get; set; }

        [Display(Name = "Postcode")]
        [StringLength(5, ErrorMessage = "Please enter a postcode with {0} digits or less.")]
        public string postCode { get; set; }

        [Required]
        [Display(Name = "Suburb")]
        [StringLength(256, ErrorMessage = "Please enter a suburb with {0} digits or less.")]
        public string suburb { get; set; }

        [Required]
        [Display(Name = "State")]
        [StringLength(256, ErrorMessage = "Please enter a state with {0} digits or less.")]
        public string state { get; set; }

        [Required]
        [Display(Name = "Country")]
        [StringLength(128, ErrorMessage = "Please enter a country with {0} digits or less.")]
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
