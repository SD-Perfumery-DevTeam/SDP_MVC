using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
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
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(SDPDbContext db, IDbRepo dbRepo, ILogger<ArticleController> logger)
        {
            _db = db;
            _dbRepo = dbRepo;
            _logger = logger;
        }

        // View Articles ======================================================
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index()
        {
            List<Article> list;

            try
            {
                list = _db.article.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in Index action method in Article controller.");
                return RedirectToAction("Error", "Home");
            }

            return View(list);
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
        public async Task<IActionResult> AddArticleAsync(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _db.article.Add(article);
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
            Article article;

            try
            {
                article = _dbRepo.GetArticle(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditArticle action method (get) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(article);
        }

        // Edit Article HTTPPOST ==============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Article articleToUpdate;

            try
            {
                articleToUpdate = _dbRepo.GetArticle(article.key);
                articleToUpdate.title = article.title;
                articleToUpdate.text = article.text;
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
