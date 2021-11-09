﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using Microsoft.SDP.SDPInfrastructure.Services;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore;

namespace SDP.Controllers
{
    public class ProductController : Controller
    {
        List<Product> pList;
        private static SDPDbContext _db;
        private IDbRepo _dbRepo;
        private ICustomer _customer = null;
        private ImageService _imageService;


        public ProductController(SDPDbContext db, IDbRepo dbRepo, ImageService imageService)
        {
            _db = db;
            _dbRepo = dbRepo;
            _imageService = imageService;
        }

        //======================Product Catelog=========================
        [HttpPost]
        [HttpGet]
        public IActionResult Index(int pageNumber = 0)
        {
            IEnumerable<Product> products;
            int totalPage;
            try
            {
                 products = _dbRepo.GetProductList().Where(m => m.isActive)
                          .Where(m => (_dbRepo.GetInventory(m.productId.ToString())>0))
                          .Skip(pageNumber * 20)
                          .Take(20);

                ViewData["Title"] = "All Products";
                totalPage = _db.product.Count() / 20;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
           

            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(guest);
                string Id = guest.Id.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            try
            {
                var brands = _db.brand.ToList();
                return View(new Catalog
                {
                    products = products,
                    brands = brands,
                    categories = _db.category.ToList(),
                    totalPage = totalPage,
                    customer = _customer
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [Route("Product/ProductDisplay/{key}")]
        [Route("Product/ProductDisplay")]
        //===============single product display=========================
        public IActionResult ProductDisplay(string key)
        {
            try
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    GuestCustomer guest = new GuestCustomer(_dbRepo);
                    GlobalVar.customerList.Add(guest);
                    string Id = guest.Id.ToString();
                    HttpContext.Session.SetString("Id", Id);
                }
                ViewData["Id"] = HttpContext.Session.GetString("Id");

                // Identify the product based on the string 'value passed in
                Product product = _dbRepo.getProduct(key);
                var lProduct = _db.product.ToList();

                HttpContext.Session.SetString("ProductID", key);
                return View(new Catalog { product = product, brand = product.brand });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        //===============add product to cart=========================
        public IActionResult AddToCart(int quantity)
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(guest);
                string Id = guest.Id.ToString();

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
            var list = GlobalVar.customerList;
            ViewService.getCustomerFromList(HttpContext.Session.GetString("Id")).cart.addProductToCart(product, quantity); //add product to guest customer

            try//increments cart count display
            {
                if (HttpContext.Session.GetString("count") == null)
                {
                    HttpContext.Session.SetString("count", quantity.ToString());
                }
                else
                {
                    var count = int.Parse(HttpContext.Session.GetString("count"));
                    HttpContext.Session.SetString("count",(count + quantity).ToString());
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Product");
        }

        //======================Product CMS=========================
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

            var model = new ViewModels.AddProduct
            {
                product = new Product(),
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
               )
            };
            return View(model);
        }

        //======================Product CMS=========================
        // This deals with the dropdown lists and img.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AddProduct(AddProduct AP, string catID, string brandID, IFormFile ufile, int stockQty)
        {
            if (ufile != null && ufile.Length > 0)
            {
                if (await _imageService.addImageToFileAsync(ufile, AP.product, _db.product.ToList()) == "Format Error") return RedirectToAction("Error", "Home");//adding image to file using image serive
            }

            try
            {
                AP.product.category = _db.category.ToList().Where(a => a.categoryId == Guid.Parse(catID)).ToList().First();
                AP.product.brand = _db.brand.ToList().Where(a => a.brandId == Guid.Parse(brandID)).ToList().First();
                _db.product.Add(AP.product);
                _db.inventory.Add(new Inventory(AP.product, AP.inventory.stockQty));

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Product");
        }


        //===================display all product in category========================
        public async Task<IActionResult> CategoryDisplay(string categoryId, int pageNumber = 0)
        {
            IEnumerable<Product> products;
            int totalPage;
            try
            {
                products = _dbRepo.GetProductList().Where(m => m.isActive).Where(m => m.category.categoryId.ToString() == categoryId)
                         .Where(m => (_dbRepo.GetInventory(m.productId.ToString()) > 0))
                         .Skip(pageNumber * 20)
                         .Take(20);
                ViewData["Title"] = string.IsNullOrWhiteSpace(_dbRepo.GetCategory(categoryId).title)? "Our Products" : _dbRepo.GetCategory(categoryId).title;
                totalPage = _db.product.Count() / 20;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }


            if (HttpContext.Session.GetString("Id") == null)
            {
                GuestCustomer guest = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(guest);
                string Id = guest.Id.ToString();
                HttpContext.Session.SetString("Id", Id);
            }
            ViewData["Id"] = HttpContext.Session.GetString("Id");

            try
            {
                
                var brands = _db.brand.ToList();
                return View("Index", new Catalog
                {
                    products = products,
                    brands = brands,
                    categories = _db.category.ToList(),
                    totalPage = totalPage,
                    customer = _customer
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}