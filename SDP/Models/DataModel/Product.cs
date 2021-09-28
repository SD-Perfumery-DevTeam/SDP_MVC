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
        // Default Constructor
        public Product() { }

        // Constructor that provides a new Guid
        public Product( string title, Category category, Consts.Genders productGender,
            decimal price, bool onSpecial, string imgSrc, Consts.PTypes productType,
            decimal packageQty, Consts.Uom packageUom, int packageWeight,
            string packageDims, Brand brand, string description, bool isActive)
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
            this.isActive = isActive;
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
        [Display(Name = "Gender")]
        public Consts.Genders productGender { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0.01, 99999999.99, ErrorMessage = "Please enter a value in the range {0} - {1}")]
        public decimal price { get; set; }

        [StringLength(2048)]
        [Display(Name = "Product Image")]
        public string imgUrl { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public Consts.PTypes productType { get; set; }

        [Required]
        [Range(0.01, 999.99, ErrorMessage = "Please enter a value in the range {0} - {1}")]
        [Display(Name = "Package Quantity")]
        public decimal packageQty { get; set; }

        [Display(Name = "Unit of Measure")]
        public Consts.Uom packageUom { get; set; }

        [Range(1, 9999, ErrorMessage = "Please enter a value in the range {0} - {1}")]
        [Display(Name = "Package Weight")]
        public int packageWeight { get; set; }

        [Display(Name = "Package Dimensions")]
        public string packageDims { get; set; }

        [StringLength(2048)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public Brand brand{ get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool isActive { get; set; }

        //prop that are not in the database 
        public bool onSpecial;
        public int count= 1;
    }
}
