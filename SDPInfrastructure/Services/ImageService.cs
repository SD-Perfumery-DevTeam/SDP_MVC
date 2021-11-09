using Microsoft.AspNetCore.Http;
using Microsoft.SDP.SDPCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPInfrastructure.Services
{
    public class ImageService
    {
        // Image service for Article objects.
        public async Task<string> addImageToFileAsync(IFormFile imgFile, Article article, List<Article> aList)
        {
            var fileName = Path.GetFileName(imgFile.FileName);
            string[] fileNameAry = fileName.Split(".");
            foreach (var a in aList)
            {
                if (a.imgUrl.Equals(fileName.Trim()))
                {
                    fileName = fileNameAry[fileNameAry.Length - 2] + "_new." + fileNameAry[fileNameAry.Length - 1];
                }
            }

            if (fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "png" && fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "jpg")
            {
                return "Format Error";
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\article", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }
            article.imgUrl = fileName;
            return fileName;
        }

        // Image service for Category objects.
        public async Task<string> addImageToFileAsync(IFormFile imgFile, Category category, List<Category> cList)
        {
            var fileName = Path.GetFileName(imgFile.FileName);
            string[] fileNameAry = fileName.Split(".");
            foreach (var c in cList)
            {
                if (c.imgUrl.Equals(fileName.Trim()))
                {
                    fileName = fileNameAry[fileNameAry.Length - 2] + "_new." + fileNameAry[fileNameAry.Length - 1];
                }
            }

            if (fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "png" && fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "jpg")
            {
                return "Format Error";
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\category", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }
            category.imgUrl = fileName;
            return fileName;
        }

        // Image service for Product objects.
        public async Task<string> addImageToFileAsync(IFormFile imgFile, Product product, List<Product> pList)
        {
            var fileName = Path.GetFileName(imgFile.FileName);
            string[] fileNameAry = fileName.Split(".");
            foreach (var p in pList)
            {
                if (p.imgUrl.Equals(fileName.Trim()))
                {
                    fileName =  fileNameAry[fileNameAry.Length - 2] + "_new." + fileNameAry[fileNameAry.Length - 1];
                }
            }

            if (fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "png" && fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "jpg")
            {
                return "Format Error";
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\product", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }
            product.imgUrl = fileName;
            return fileName;
        }

        // Image service for Promotion objects.
        public async Task<string> addImageToFileAsync(IFormFile imgFile, Promotion promotion, List<Product> pList)
        {
            var fileName = Path.GetFileName(imgFile.FileName);
            string[] fileNameAry = fileName.Split(".");
            foreach (var p in pList)
            {
                if (p.imgUrl.Equals(fileName.Trim()))
                {
                    fileName = fileNameAry[fileNameAry.Length - 2] + "_new." + fileNameAry[fileNameAry.Length - 1];
                }
            }

            if (fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "png" && fileNameAry[fileNameAry.Length - 1].Trim().ToLower() != "jpg")
            {
                return "Format Error";
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\promotion", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }
            promotion.imgUrl = fileName;
            return fileName;
        }
    }
}
