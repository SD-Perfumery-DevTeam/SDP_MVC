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
    }
}
