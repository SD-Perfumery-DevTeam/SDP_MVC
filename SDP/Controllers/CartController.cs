using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SDP.Extensions;
using SDP.Interfaces;
using SDP.Models;
using SDP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                Global.customerList.Add(guest); 
                string Id = guest.userId.ToString();
                //ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"))
                HttpContext.Session.SetString("Id", Id);
            }
            
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            customer = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"));
            return View();
        }
    }
}
