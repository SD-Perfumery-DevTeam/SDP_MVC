
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContext;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPInfrastructure.Services;
using SDPWeb.ViewModels;

namespace SDP.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class InventoryController : Controller
    {
        
        List<Product> pList;
        private static ProductDbContext _db;
        private ImageService _imageService;
        ICustomer customer = null;
        private IDbRepo _dbRepo;

        public InventoryController(ProductDbContext db, ImageService imageService, IDbRepo dbRepo)
        {
            _db = db;
            _imageService = imageService;
            _dbRepo = dbRepo;
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
                if (await _imageService.addImageToFileAsync(ufile, AP.product, _db.product.ToList()) == "Format Error") return RedirectToAction("Error", "Home");//adding image to file using image serive
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
        public async Task<IActionResult> DeleteProduct(string ProductId) 
        {
            
            try 
            {
                Guid productID = Guid.Parse(ProductId);
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

        //===================List out promotions=======================
        public IActionResult ViewPromotions() 
        {
            var list = _db.promotion.Include(m => m.product).ToList<Promotion>();
            return View(new AddPromotionView { promotionList = list});
        }
        //===================Add promotion page HTTPPOST=======================
        [HttpPost]
        public IActionResult AddPromotion(string productId)
        {
            return View(new AddPromotionView { promotion = new Promotion { product = _dbRepo.getProduct(productId) }}); 
        }

        //===================Add promotion page HTTPGET=======================
        [HttpGet]
        public IActionResult AddPromotion(string productId, string ErrorMsg)
        {
            ViewData["Error"] = ErrorMsg;
            return View(new AddPromotionView { promotion = new Promotion { product = _dbRepo.getProduct(productId) } });
        }

        //===================Add promotion to the db checking for duplicate codes=======================
        [HttpPost]
        public IActionResult AddPromotionToDb(string productId, AddPromotionView model )
        {
            try
            {
                Promotion promotion = model.promotion;
                if (_dbRepo.getPromotion(promotion.promoCode) != null)
                {
                    return RedirectToAction("AddPromotion", "Inventory", new { productId = productId, ErrorMsg = "Duplicate Promotion Code, please try a different code" });
                }
                else if (promotion.startDate > promotion.endDate)
                {
                    return RedirectToAction("AddPromotion", "Inventory", new { productId = productId, ErrorMsg = "Start Date cannot be later than the end date" });
                }
                promotion.product = _dbRepo.getProduct(productId);

                _db.promotion.Add(promotion);
                _db.SaveChanges();

                var list = _db.promotion.Include(m => m.product).ToList<Promotion>();
                return RedirectToAction("ViewPromotions", new AddPromotionView { promotionList = list });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
        }
    }
}
