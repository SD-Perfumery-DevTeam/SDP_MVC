using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SDP.Interfaces;
using SDP.Models;
using SDP.Models.DbContext;
using SDP.Services;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<IActionResult> Index() 
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

            try
            {
                return View(new Catalog
                {
                    products = _db.product.ToList(),
                    brands = _db.brand.ToList()
                });
            }
            catch (Exception ex) 
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult ProductDisplay(string value)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.userId.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");
            // Identify the product based on the string 'value passed in
            Guid productID = Guid.Parse(value); // this appears to throw an unhandled exception at times..
            Product product = _db.product.Include(x => x.brand).FirstOrDefault(x => x.productId.Equals(productID));
            var lProduct = _db.product.ToList();

            HttpContext.Session.SetString("ProductID", productID.ToString());

            try
            {
                return View(new Catalog { product = product, brand = product.brand });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

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
            try
            {
                product = _db.product.Find(Guid.Parse(HttpContext.Session.GetString("ProductID")));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            
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
            List<Category> c;
            List<Brand> b;
            try
            {
                c = _db.category.ToList();
                b = _db.brand.ToList();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            
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

        // This deals with the dropdown lists  and img.
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProduct P, string catID, string brandID, IFormFile ufile)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var fileName = Path.GetFileName(ufile.FileName);
                string[] fileNameAry = fileName.Split(".");

                if (fileNameAry[fileNameAry.Length - 1] != "png" &&  fileNameAry[fileNameAry.Length - 1] != "jpg") 
                {
                    return RedirectToAction("Error", "Home");
                }
                P.product.imgUrl = fileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\imgs", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
            }

            try
            {
                P.product.category = _db.category.ToList().Where(a => a.categoryId == Guid.Parse(catID)).ToList().First();
                P.product.brand = _db.brand.ToList().Where(a => a.brandId == Guid.Parse(brandID)).ToList().First();
                _db.product.Add(P.product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Product");
        }      
    }
}