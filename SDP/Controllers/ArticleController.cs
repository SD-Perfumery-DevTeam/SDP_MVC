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
        [Route("Article/{key}")]
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

        // Add Category HTTPGET ===============================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddArticle()
        {
            return View();
        }

        // Add Category HTTPPOST ==============================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _db.article.Add(article);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in AddArticle action method (post) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        // Edit Category HTTPGET ==============================================
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditArticle(string articleId)
        {
            Article article;

            try
            {
                article = _dbRepo.GetArticle(articleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem in EditArticle action method (get) in Article controller.");
                return RedirectToAction("Error", "Home");
            }
            return View(article);
        }

        // Edit Category HTTPPOST =============================================
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
                articleToUpdate = _db.article.Find(article.articleId);
                articleToUpdate.articleId = article.articleId;
                articleToUpdate.title = article.title;
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

        // Delete Category HTTPPOST ===========================================
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteArticle(string articleId)
        {
            Guid guidToDelete;
            Article articleToDelete;

            try
            {
                guidToDelete = Guid.Parse(articleId);
                articleToDelete = _db.article.Find(guidToDelete);
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
