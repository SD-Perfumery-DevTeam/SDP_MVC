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
    public class ProductController : Controller
    {
        Customer customer = null;
        public string productID { get; set; }
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
        public IActionResult ProductDisplay(string value)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                Models.GuestCustomer guest = new Models.GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.ID.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            productID = value;

            ViewData["ProductID"] = productID;
            HttpContext.Session.SetString("ProductID", productID);
            return View();
        }
        
        public IActionResult AddToCart(int quantity) 
        {
            Product product = null;
            product = ViewService.getProductFromDB(HttpContext.Session.GetString("ProductID"));
            for (int i = 0; i < quantity; i++)
            {
                ViewService.getCustomerFromDB(HttpContext.Session.GetString("Id")).cart.addToCart(product);
            }
            return RedirectToAction("Index", "Product"); ;
        }
    }
}
