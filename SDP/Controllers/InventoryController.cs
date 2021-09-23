
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SDP.Interfaces;
using SDP.Models;
using SDP.Models.DbContext;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class InventoryController : Controller
    {
        
        List<Product> pList;
        private static ProductDbContext _db;

        ICustomer customer = null;

        public InventoryController(ProductDbContext db)
        {
            _db = db;
        }
        //===================Inventory display page=======================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index()
        {
            List<Inventory> list;
            try
            {
                list = (_db.inventory.Include(m => m.product)).ToList();
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
            return View(new InventoryView { inventories = list });
        }

        //===================this method displays the change page=======================
      
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditProduct(string productId)
        {
            Guid productID = Guid.Parse(productId);
            Product product;
            Inventory inventory;
            try
            {
                product = _db.product.Include(m => m.brand).Include(m => m.category).FirstOrDefault(x => x.productId.Equals(productID));
                inventory = _db.inventory.Include(m => m.product).ToList().Where(i => i.product.productId == product.productId).FirstOrDefault(x => x.product.productId.Equals(productID));
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
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
                product = product,
                inventory = inventory,
                categories = c.Select(x => new SelectListItem
                {
                    Value = x.categoryId.ToString(),
                    Text = x.title
                }),
                brands = b.Select(i => new SelectListItem
                {
                    Value = i.brandId.ToString(),
                    Text = i.title
                })
            };

            return View(model);
        }
        //===================Update Product=======================
        //this method saves the changes to the database
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateProduct(AddProduct AP, string productId, string InvenId, string catID, string brandID, IFormFile ufile, string Url)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var fileName = Path.GetFileName(ufile.FileName);
                string[] fileNameAry = fileName.Split(".");

                if (fileNameAry[fileNameAry.Length - 1] != "png" && fileNameAry[fileNameAry.Length - 1] != "jpg")
                {
                    return RedirectToAction("Error", "Home");
                }
                AP.product.imgUrl = fileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\imgs", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
            }
            else AP.product.imgUrl = Url;

            try
            {
                AP.product.productId = Guid.Parse(productId);
                AP.inventory.inventoryId = Guid.Parse(InvenId);
                AP.product.category = _db.category.ToList().Where(a => a.categoryId == Guid.Parse(catID)).ToList().First();
                AP.product.brand = _db.brand.ToList().Where(a => a.brandId == Guid.Parse(brandID)).ToList().First();
                AP.inventory.product = AP.product;
                _db.Update(AP.product);
                _db.Update(AP.inventory);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Inventory");

        }
        //===================DeleteProduct=======================
        //this deletes the product based on product ID
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteProduct(string ProdcutId) 
        {
            
            try 
            {
                Guid productID = Guid.Parse(ProdcutId);
                _db.product.Remove(_db.product.Find(productID));
                _db.inventory.Remove(_db.inventory.Include(m => m.product).ToList().Where(i => i.product.productId == _db.product.Find(productID).productId).FirstOrDefault(x => x.product.productId.Equals(productID)));
                await _db.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return RedirectToAction("Error", "Home");
            }
           
            return RedirectToAction("Index", "Inventory");
        }
    }
}
