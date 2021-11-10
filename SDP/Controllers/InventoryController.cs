using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPInfrastructure.Services;
using SDPWeb.ViewModels;
using Microsoft.Extensions.Logging;

namespace SDP.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class InventoryController : Controller
    {
        List<Product> pList;
        private SDPDbContext _db;
        private ImageService _imageService;
        ICustomer customer = null;
        private IDbRepo _dbRepo;
        private readonly IDbContextFactory<SDPDbContext> _contextFactory;
        private readonly ILogger<HomeController> _logger;

        public InventoryController(SDPDbContext db, ImageService imageService, IDbRepo dbRepo, IDbContextFactory<SDPDbContext> contextFactory, ILogger<HomeController> logger)
        {
            _db = db;
            _imageService = imageService;
            _dbRepo = dbRepo;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        // Inventory Display Page =============================================
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
                _logger.LogError(ex, "Problem loading Index view in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(new InventoryView { inventories = list });
        }

        // This method displays the change page ===============================
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
                _logger.LogError(ex, "Problem displaying Product object: EditProduct view (HttpPost) in Inventory controller.");
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
                _logger.LogError(ex, "Problem displaying Category / Brand object: EditProduct view (HttpPost) in Inventory controller.");
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

        // Update Product =====================================================
        // This method saves the changes to the database.
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdateProduct(AddProduct AP, string productId, string InvenId, string catID, string brandID, IFormFile ufile, string Url)
        {
            if (ufile != null && ufile.Length > 0)
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    if (await _imageService.addImageToFileAsync(ufile, AP.product, context.product.ToList()) == "Format Error") return RedirectToAction("Error", "Home");//adding image to file using image serive
                }

            }
            else AP.product.imgUrl = Url;

            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {

                    AP.product.productId = Guid.Parse(productId);
                    AP.inventory.inventoryId = Guid.Parse(InvenId);
                    AP.product.category = context.category.ToList().Where(a => a.categoryId == Guid.Parse(catID)).ToList().First();

                    AP.product.brand = context.brand.ToList().Where(a => a.brandId == Guid.Parse(brandID)).ToList().First();
                    AP.inventory.product = AP.product;
                }
                _db.Update(AP.product);
                _db.Update(AP.inventory);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem updating Product object: UpdateProduct view (HttpPost) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index", "Inventory");
        }
        
        // Delete Product =====================================================
        //this deletes the product based on product ID
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteProduct(string ProductId)
        {

            try
            {
                Guid productID = Guid.Parse(ProductId);
                _db.product.Remove(_db.product.Find(productID));
                _db.inventory.Remove(_db.inventory.Include(m => m.product).ToList().Where(i => i.product.productId ==
                _db.product.Find(productID).productId).FirstOrDefault(x => x.product.productId.Equals(productID)));
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem deleting Product object: DeleteProduct view (HttpPost) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Inventory");
        }

        // List out promotions ================================================
        public IActionResult ViewPromotions()
        {
            var list = _db.promotion.Include(m => m.product).ToList<Promotion>();
            return View(new AddPromotionView { promotionList = list });
        }
        // Add promotion page HTTPPOST ========================================
        [HttpPost]
        public IActionResult AddPromotion(string productId)
        {
            return View(new AddPromotionView { promotion = new Promotion { product = _dbRepo.getProduct(productId) } });
        }

        // Add promotion page HTTPGET =========================================
        [HttpGet]
        public IActionResult AddPromotion(string productId, string ErrorMsg)
        {
            ViewData["Error"] = ErrorMsg;
            return View(new AddPromotionView { promotion = new Promotion { product = _dbRepo.getProduct(productId) } });
        }

        // Add promotion to the db checking for duplicate codes ===============
        [HttpPost]
        public async Task<IActionResult> AddPromotionToDbAsync(string productId, IFormFile ufile, AddPromotionView model)
        {
            try
            {
                if (ufile != null && ufile.Length > 0)
                {
                    if (await _imageService.addImageToFileAsync(ufile, model.promotion, _db.product.ToList()) == "Format Error") return RedirectToAction("Error", "Home");//adding image to file using image serive
                }

                Promotion promotion = model.promotion;
                if (_dbRepo.getPromotionBypromoCode(promotion.promoCode) != null)
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
                _logger.LogError(ex, "Problem adding Promotion object: AddPromotionToDbAsync view (HttpPost) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult EditPromotion(string promoId)
        {
            try
            {
                return View(new AddPromotionView { promotion = _dbRepo.getPromotionByID(promoId) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem editing Promotion object: EditPromotion view (HttpPost) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePromotionDbAsync(string productId, string promoId, IFormFile ufile, AddPromotionView model)
        {
            try
            {
                model.promotion.promoId = !string.IsNullOrWhiteSpace(promoId) ? Guid.Parse(promoId) : Guid.NewGuid();
                if (ufile != null && ufile.Length > 0)
                {
                    using (var context = _contextFactory.CreateDbContext())
                    {
                        if (await _imageService.addImageToFileAsync(ufile, model.promotion, context.product.ToList()) == "Format Error") return RedirectToAction("Error", "Home");//adding image to file using image serive
                    }

                }

                using (var context = _contextFactory.CreateDbContext())
                {
                    model.promotion.product = await context.product.FindAsync(Guid.Parse(productId));
                }

                _db.promotion.Update(model.promotion);
                await _db.SaveChangesAsync();

                using (var context = _contextFactory.CreateDbContext())
                {
                    var list = context.promotion.Include(m => m.product).ToList<Promotion>();
                    return RedirectToAction("ViewPromotions", new AddPromotionView { promotionList = list });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem updating Promotion object: UpdatePromotionToDbAsync view (HttpPost) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
        }

        // View Brands ========================================================
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ViewBrands()
        {
            List<Brand> list;

            try
            {
                list = _db.brand.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in ViewBrands action method in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(list);
        }

        // Add Brand HTTPGET ==================================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddBrand()
        {
            return View();
        }

        // Add Brand HTTPPOST =================================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddBrand(Brand brand)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _db.brand.Add(brand);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in AddBrand action method (get) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewBrands");
        }

        // Edit Brand HTTPGET =================================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditBrand(string brandId)
        {
            Brand brand;

            try
            {
                brand = _dbRepo.GetBrand(brandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditBrand action method (get) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(brand);
        }

        // Edit Brand HTTPPOST ================================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditBrand(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Brand brandToUpdate;

            try
            {
                brandToUpdate = _db.brand.Find(brand.brandId);
                brandToUpdate.brandId = brand.brandId;
                brandToUpdate.title = brand.title;
                _db.Entry(brandToUpdate).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditBrand action method (post) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewBrands");
        }

        // Delete Brand HTTPPOST ==============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteBrand(string brandId)
        {
            Guid guidToDelete;
            Brand brandToDelete;
            bool assocRecordsExist;

            try
            {
                guidToDelete = Guid.Parse(brandId);
                brandToDelete = _db.brand.Find(guidToDelete);

                // 1. Check that there are no associated products.
                using (var context = _contextFactory.CreateDbContext())
                {
                     assocRecordsExist = context.product.Any(p => p.brand == brandToDelete);
                }

                // 2. If possible, delete the brand.
                if (!assocRecordsExist)
                {
                    _logger.LogInformation(brandToDelete.title + " can be safely deleted.");
                    _db.brand.Remove(brandToDelete);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in DeleteBrand action method (post) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewBrands");
        }

        // View Categories ====================================================
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ViewCategories()
        {
            List<Category> list;

            try
            {
                list = _db.category.ToList<Category>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in ViewCategories action method in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }

            return View(new AddEditCategory { categoryList = list });
        }

        // Add Category HTTPGET ===============================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddCategory()
        {
            return View();
        }

        // Add Category HTTPPOST ==============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AddCategory(AddEditCategory addEditCategory, IFormFile ufile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Use the image service to add an image file to the Category object.
            if (ufile != null && ufile.Length > 0)
            {
                try
                {
                    await _imageService.addImageToFileAsync(ufile, addEditCategory.category, _db.category.ToList());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to parse imgUrl string for AddCategory action method (post) in Inventory controller.");
                    return RedirectToAction("Error", "Home");
                }
            }

            try
            {
                _db.category.Add(addEditCategory.category);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in AddCategory action method (post) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewCategories");
        }

        // Edit Category HTTPGET ==============================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditCategory(string categoryId)
        {
            AddEditCategory returnCategory;

            try
            {
                returnCategory = new AddEditCategory { category = _dbRepo.GetCategory(categoryId) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditCategory action method (get) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(returnCategory);
        }

        // Edit Category HTTPPOST =============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> EditCategory(AddEditCategory addEditCategory, string categoryId, IFormFile ufile, string Url)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Use the image service to add an image file to the Category object.
            if (ufile != null && ufile.Length > 0)
            {
                try
                {
                    await _imageService.addImageToFileAsync(ufile, addEditCategory.category, _db.category.ToList());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to parse imgUrl string for EditCategory action method (post) in Inventory controller.");
                    return RedirectToAction("Error", "Home");
                }
            }
            else addEditCategory.category.imgUrl = Url; // If the image field is left, use the previous image.

            Category categoryToUpdate;

            try
            {
                categoryToUpdate = _dbRepo.GetCategory(categoryId);
                categoryToUpdate.imgUrl = addEditCategory.category.imgUrl;
                categoryToUpdate.title = addEditCategory.category.title;
                _db.Entry(categoryToUpdate).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditCategory action method (post) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewCategories");
        }

        // Delete Category HTTPPOST ===========================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteCategory(string categoryId)
        {
            Guid guidToDelete;
            Category categoryToDelete;
            bool assocRecordsExist;

            try
            {
                guidToDelete = Guid.Parse(categoryId);
                categoryToDelete = _db.category.Find(guidToDelete);

                // 1. Check that there are no associated products.
                using (var context = _contextFactory.CreateDbContext())
                {
                    assocRecordsExist = context.product.Any(p => p.category == categoryToDelete);
                }

                // 2. If possible, delete the category.
                if (!assocRecordsExist)
                {
                    _logger.LogInformation(categoryToDelete.title + " can be safely deleted.");
                    _db.category.Remove(categoryToDelete);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in DeleteCategory action method (post) in Inventory controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ViewCategories");
        }
    }
}
