using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPInfrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore;

namespace SDP.Controllers
{
    public class CartController : Controller
    {
        ICustomer customer = null;

        public IActionResult Index()
        {
            //checking for valid id in session data creates a new one if none exist
            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer();
                GlobalVar.customerList.Add(guest); 
                string Id = guest.userId.ToString(); 
                HttpContext.Session.SetString("Id", Id);
            }
            
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
            return View();
        }
    }
}
