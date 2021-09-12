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
            [Display(Name = "Mens")]
            male,
            [Display(Name = "Womens")]
            female,
            [Display(Name = "Unisex")]
            other
        };
        public enum Status
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


