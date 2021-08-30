using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        Customer customer = null;
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.ID.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            customer = ViewService.getCustomerFromDB(HttpContext.Session.GetString("Id"));
            return View();
        }
    }
}
