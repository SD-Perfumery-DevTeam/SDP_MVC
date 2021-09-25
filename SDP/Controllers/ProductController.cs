using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SDP.Extensions;
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
        private IDbRepo _dbRepo;
        ICustomer customer = null;

        public ProductController(ProductDbContext db, IDbRepo dbRepo)
        {
            _db = db;
            _dbRepo = dbRepo;
        }

        //displays the product catelog
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 0) 
        {

            var products = _db.product
                          .Skip(pageNumber * 20)
                          .Take(20);
            int totalPage = _db.product.Count()/20;

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

            try
            {
                return View(new Catalog
                {
                    products = products,
                    brands = _db.brand.ToList(),
                    totalPage =totalPage
                });
            }
            catch (Exception ex) 
            {
                return RedirectToAction("Error", "Home");
            }
        }

        //single product display
        public IActionResult ProductDisplay(string value)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.userId.ToString();
                //ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"))
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            // Identify the product based on the string 'value passed in
            Product product = _dbRepo.getProduct(value);
            var lProduct = _db.product.ToList();

            HttpContext.Session.SetString("ProductID", value);

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
                GuestCustomer guest = new GuestCustomer();
                Global.customerList.Add(guest);
                string Id = guest.userId.ToString();
                //ViewService.getCustomerFromList(HttpContext.Session.GetString("Id"))
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            Product product = null;
            try
            {
                product = _dbRepo.getProduct(HttpContext.Session.GetString("ProductID"));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
           
            ViewService.getCustomerFromList(HttpContext.Session.GetString("Id")).cart.addProductToCart(product, quantity); //add product to guest customer
          
            return RedirectToAction("Index", "Product"); 
        }

        //Product CMS
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
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
        [Authorize(Roles = "Admin, SuperAdmin")]
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
               await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Product");
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        public IActionResult AddBrand()
        {
            return View();
        }

       
    }
}