using SDP.Interfaces;
using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SDP
{
    //replace data access class when ready
    public static class MockDB
    {
        public static List<Product> MockProductDB = new List<Product>() { 
        new Product("Bright Crystal", (decimal)4500.54, false, "img5.jpg", Consts.PTypes.EDT, 30, "Versace","test string"),
        new Product("Light Blue", (decimal)6798.54, false, "img6.jpg", Consts.PTypes.EDP, 50, "Dolce & Gabbana","test string"),
        new Product("Test-3 ", (decimal)3530.54, false, "img3.jpg", Consts.PTypes.EDT, 30, "CD","test string"),
        new Product("Test-4", (decimal)8500.54, false, "img4.jpg", Consts.PTypes.EDP, 30, "Gucci","test string"),
        };
        public static List<IUser> MockUserDB = new List<IUser>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        static List<Tuple<Product, int>> stock;

        public static void popStock() 
        {
            foreach (Product p in MockDB.MockProductDB) 
            {
                stock.Add(new Tuple<Product, int> (p,100));
            }
        }

        public static int findStock(Product p) 
        {
            foreach (Tuple<Product, int> t in stock) 
            {
                if (t.Item1 == p) return t.Item2;
            }
            return 0;
        }

        public static string productListTOJSON() 
        {
            return JsonSerializer.Serialize(MockProductDB);
        }

        public static string TestproductListTOJSON()
        {
            return JsonSerializer.Serialize(MockProductDB.First());
        }
    }
}
