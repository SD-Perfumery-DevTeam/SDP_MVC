using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.SDP.SDPCore.Models.DbContexts;

namespace SDPWeb.Controllers
{
    public class RecommendedProductsViewComponent : ViewComponent
    {
        private SDPDbContext _db;

        public RecommendedProductsViewComponent(SDPDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Product> products = _db.product.Include(m=>m.brand).ToList();
            List<Product> reconmmendedProducts = new List<Product>();
            foreach (var index in getRandomNum(products))
            {
                reconmmendedProducts.Add(products[index]);
            }
            return await Task.FromResult((IViewComponentResult)View("RecommendedProducts", reconmmendedProducts));
        }

        public List<int> getRandomNum(List<Product> list) 
        {
            List<int> numbers = new List<int>();
            Random random = new Random();
            while (numbers.Count < 3) 
            {
                int index = random.Next(list.Count);
                if (!numbers.Contains(index))
                {
                    numbers.Add(index);
                }
            }
            return numbers;
        }
    }
}
