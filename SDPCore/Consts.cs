using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore
{
    public static class Consts
    {
        public enum PTypes
        {
            EDP,
            EDT
        };

        public enum Genders
        {
            [Display(Name = "Men's")]
            male,
            [Display(Name = "Women's")]
            female,
            [Display(Name = "Unisex")]
            other
        };

        public enum DeliveryStatus
        {
            [Display(Name = "Action Required")]
            pendingAction,
            [Display(Name = "Out for Delivery")]
            beingDelivered,
            [Display(Name = "Delivered")]
            deliveryComplete
        };
        public enum OrderStatus
        {
            [Display(Name = "Pending Action")]
            pendingAction,
            Actioned,
            Error
        };
        public enum Uom 
        {
            ml,
            g,
            L,
            Kg,
            Oz,
        }
    }
}


