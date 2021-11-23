using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SDP
{
    public static class Consts
    {
        // Method to return the user readable display name when available.
        // Reference: https://benjaminray.com/codebase/c-enum-display-names-with-spaces-and-special-characters/
        public static string GetDisplayName(this Enum enumValue)
        {
            string displayName;
            displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();

            if (String.IsNullOrEmpty(displayName))
            {
                displayName = enumValue.ToString();
            }
            
            return displayName;
        }
        
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


