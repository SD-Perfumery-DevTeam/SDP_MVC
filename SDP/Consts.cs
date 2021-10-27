using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDP
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
            pendingAction,
            beingDelivered,
            deliveryComplete
        };

        public enum OrderStatus
        {
            pendingAction,
            beingDelivered,
            deliveryComplete
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


