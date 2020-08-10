using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace InitCMS.ViewModel
{
    public class DailySaleViewModel
    { 
        public string ProductName { get; set; }
        public string PCode { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order Date")]
        [BindRequired]
        public DateTime Date { get; set; }
        
    }
}
