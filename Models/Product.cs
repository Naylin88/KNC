using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InitCMS.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [DisplayName("Code")]
        [Remote(action: "CheckPCode", controller: "Products")]
        public string PCode { get; set; }
        public string Description { get; set; }
        [DisplayName("Cost")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal PurchasePrice { get; set; }
        [DisplayName("Price")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? SellPrice { get; set; }
        [DisplayName("In Stock")]
        public int? InStock { get; set; }
        [DisplayName("Sale")]
        public int? Sale { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        public int ProductCategoryID { get; set; }
        public int CategoryCatId { get; set; }
        [DisplayName("Photo")]
        public string ImagePath { get; set; }
        [DisplayName("Sub Category")]
        public ProductCategory ProductCategory { get; set; }
        public Category Category { get; set; }
        
    }
}
