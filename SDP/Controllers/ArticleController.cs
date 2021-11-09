using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using Microsoft.SDP.SDPInfrastructure.Services;
using SDPWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDPWeb.Controllers
{
    public class ArticleController : Controller
    {
        private SDPDbContext _db;
        private IDbRepo _dbRepo;
        private ImageService _imageService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(SDPDbContext db, IDbRepo dbRepo, ImageService imageService, ILogger<ArticleController> logger)
        {
            _db = db;
            _dbRepo = dbRepo;
            _imageService = imageService;
            _logger = logger;
        }

        // View Articles ======================================================
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index()
        {
            List<Article> list;

            try
            {
                list = _db.article.ToList<Article>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in Idex action method in Article controller.");
                return RedirectToAction("Error", "Home");
            }

            return View(new AddEditArticle { articleList = list });
        }

        // View Single Article ================================================
        [Route("Article/View/{key}")]
        public IActionResult Article(string key)
        {
            Article article;
            try
            {
                article = _dbRepo.GetArticle(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in Article action method in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(article);
        }

        // Add Article HTTPGET ================================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddArticle()
        {
            return View();
        }

        // Add Article HTTPPOST ===============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AddArticleAsync(AddEditArticle addEditArticle, IFormFile ufile)
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
                    await _imageService.addImageToFileAsync(ufile, addEditArticle.article, _db.article.ToList());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to parse imgUrl string for AddArticle action method (post) in Article controller.");
                    return RedirectToAction("Error", "Home");
                }
            }

            try
            {
                _db.article.Add(addEditArticle.article);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in AddArticle action method (post) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        // Edit Article HTTPGET ===============================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditArticle(string key)
        {
            AddEditArticle returnArticle;

            try
            {
                returnArticle = new AddEditArticle { article = _dbRepo.GetArticle(key) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditArticle action method (get) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(returnArticle);
        }

        // Edit Article HTTPPOST ==============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> EditArticle(AddEditArticle addEditArticle, IFormFile ufile)
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
                    await _imageService.addImageToFileAsync(ufile, addEditArticle.article, _db.article.ToList());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to parse imgUrl string for EditArticle action method (post) in Article controller.");
                    return RedirectToAction("Error", "Home");
                }
            }

            Article articleToUpdate;

            try
            {
                articleToUpdate = _dbRepo.GetArticle(addEditArticle.article.key);
                articleToUpdate.title = addEditArticle.article.title;
                articleToUpdate.imgUrl = addEditArticle.article.imgUrl;
                articleToUpdate.text = addEditArticle.article.text;
                _db.Entry(articleToUpdate).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditArticle action method (post) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        // Delete Article HTTPPOST ============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteArticle(string key)
        {
            Article articleToDelete;

            try
            {
                articleToDelete = _dbRepo.GetArticle(key);
                _db.article.Remove(articleToDelete);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in DeleteArticle action method (post) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
