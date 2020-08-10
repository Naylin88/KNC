using InitCMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InitCMS.ViewModel
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}",  ApplyFormatInEditMode = true)]
        public decimal ShoppingCartTotal { get; set; }
    }
}
