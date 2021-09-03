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
        private ProductDbContext _db;

        ICustomer customer = null;

        public ProductController(ProductDbContext db)
        {
            _db = db;
        }

        public string productID { get; set; }
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
           
            
            return View(new Catalog { products = _db.product.ToList() });
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
            return RedirectToAction("Index", "Product"); 
        }

        [HttpGet]
        public IActionResult AddProduct() 
        {
            var c = _db.category.ToList();
            var model = new ViewModels.AddProduct { product = new Product(),
                categories = c.Select(x => new SelectListItem
                {
                    Value = x.categoryId.ToString(),
                    Text = x.title
                })};

            return View(model);
        }

        [HttpPost]
        public IActionResult AddProduct(AddProduct P, string catID)
        {
            P.product.category = _db.category.ToList().Where(a => a.categoryId == Guid.Parse(catID)).ToList().First();
            _db.product.Add(P.product);
            _db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

       
    }
}

//code used for populating db with mock data
/*foreach (Product p in MockDB.MockProductDB) 
{
    _db.product.Add(p);
    _db.SaveChanges();
}*/