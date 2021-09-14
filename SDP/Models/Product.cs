using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Models
{
    //this is the product model.... duh...
    [Serializable]
    public class Product 
    {
        //defualt constructor used for EF
        public Product() { }
        public Product(Guid id,string name, decimal price, bool onSpecial,
         string imgSrc, Consts.PTypes pType,
        decimal size, Brand brand, string description,int count)
        {
            this.productId = id;
            this.title = name;
            this.price = price;
            this.onSpecial = onSpecial;
            this.imgUrl = imgSrc;
            this.productType = pType;
            this.packageQty = size;
            this.brand = brand;
            this.description = description;
            this.count = count;
        }

        public Product( string title, Category category, Consts.Genders productGender, decimal price, bool onSpecial, string imgSrc, Consts.PTypes productType, decimal packageQty, Consts.Uom packageUom, int packageWeight, string packageDims, Brand brand, string description)
        {

            this.productId = Guid.NewGuid();
            this.title = title;
            this.category = category;
            this.productGender = productGender;
            this.price = price;
            this.onSpecial = onSpecial;
            this.imgUrl = imgSrc;
            this.productType = productType;
            this.packageQty = packageQty;
            this.packageUom = packageUom;
            this.packageWeight = packageWeight;
            this.packageDims = packageDims;
            this.brand = brand;
            this.description = description;
        }

        public Guid productId { get; set; }

        [Required]
        [StringLength(2048)]
        [Display(Name = "Product Name")]
        public string title { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public Category category { get; set; }

        [Required]
        [Display(Name = "Target Gender")]
        public Consts.Genders productGender { get; set; }

        [Required]
        
        public decimal price { get; set; }

        [StringLength(2048)]
        [Display(Name = "Product Image (png and jpg files only)")]
        public string imgUrl { get; set; }

        [Required]
        public Consts.PTypes productType { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public decimal packageQty { get; set; }

        public Consts.Uom packageUom { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int packageWeight { get; set; }

        [Display(Name = "Package Dimentions( In the format of length x width x height in mm )")]
        public string packageDims { get; set; }

        [StringLength(2048)]
        public string description { get; set; }

        [Required]
        public Brand brand{ get; set; }

        //prop that are not in the database 
        public bool onSpecial;
        public int count= 1;
    }
}
