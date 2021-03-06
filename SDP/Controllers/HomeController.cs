using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using SDPWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDbRepo _dbRepo;
        private readonly IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> _contextFactory;
        private SDPDbContext _db;

        public HomeController(ILogger<HomeController> logger, IDbRepo dbRepo, IDbContextFactory<SDPDbContext> contextFactory, SDPDbContext db)
        {
            _logger = logger;
            _dbRepo = dbRepo;
            _contextFactory = contextFactory;
            _db = db;
        }

        public IActionResult Index()
        {
            var homeView = new HomeView();
            var categorList = _db.category.ToList();
            var promoList = _db.promotion.Include(m=>m.product).ToList();
           

            homeView.categories = new List<Category>();
            homeView.promotions = new List<Promotion>();
            foreach (Product p in _dbRepo.GetProductList()) 
            {
                foreach (var item in categorList) 
                {
                    if (p.category.categoryId == item.categoryId && !homeView.categories.Contains(item))
                    {
                        homeView.categories.Add(item);
                    }
                }
            }
            foreach (Promotion p in promoList)
            {

                if (p.isActive && p.startDate<= DateTime.Now && p.endDate>= DateTime.Now )
                {
                    homeView.promotions.Add(p);
                }
                if (homeView.promotions.Count>= 3)
                {
                    break;
                }
            }
            

            return View(homeView);
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

     
    }
}
