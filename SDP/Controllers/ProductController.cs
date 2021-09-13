using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SDP.Interfaces;
using SDP.Models;
using SDP.Models.DbContext;
using SDP.Services;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SDP.Controllers
{
    public class ProductController : Controller
    {
        List<Product> pList;
        private static ProductDbContext _db;

        ICustomer customer = null;

        public ProductController(ProductDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.userId.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            customer = ViewService.getCustomerFromDB(HttpContext.Session.GetString("Id"));

            return View(new Catalog {
                products = _db.product.ToList(),
                brands = _db.brand.ToList()
            });
        }

        public IActionResult ProductDisplay(string value)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                Models.GuestCustomer guest = new Models.GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.userId.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            Guid productID = Guid.Parse(value); // this appears to throw an unhandled exception at times..
            Guid brandID = Guid.Parse("051f6ad7-59bf-4a9c-9d8d-06b485cedbf1");

            Product product = _db.product.Find(productID);
            Brand brand = _db.brand.Find(brandID);

            HttpContext.Session.SetString("ProductID", productID.ToString());

            return View(new Catalog { product = product, brand = brand });
        }
        
        public IActionResult AddToCart(int quantity) 
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                Models.GuestCustomer guest = new Models.GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.userId.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            Product product = null;
            product = _db.product.Find(Guid.Parse(HttpContext.Session.GetString("ProductID")));
            for (int i = 0; i < quantity; i++)
            {
                ViewService.getCustomerFromDB(HttpContext.Session.GetString("Id")).cart.addToCart(product);
            }
            return RedirectToAction("Index", "Product"); 
        }

        //Product CMS
        [HttpGet]
        public IActionResult AddProduct() 
        {
            var c = _db.category.ToList();
            var b = _db.brand.ToList();
            var model = new ViewModels.AddProduct { product = new Product(),
                categories = c.Select(x => new SelectListItem
                {
                    Value = x.categoryId.ToString(),
                    Text = x.title
                }),
                brands = b.Select(i => new SelectListItem
                {
                    Value = i.brandId.ToString(),
                    Text = i.title
                }
                
               )};

            return View(model);
        }

        // This deals with the category dropdown list.
        [HttpPost]
        public IActionResult AddProduct(AddProduct P, string catID, string brandID)
        {
            P.product.category = _db.category.ToList().Where(a => a.categoryId == Guid.Parse(catID)).ToList().First();
            P.product.brand = _db.brand.ToList().Where(a => a.brandId == Guid.Parse(brandID)).ToList().First();
            _db.product.Add(P.product);
            _db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}