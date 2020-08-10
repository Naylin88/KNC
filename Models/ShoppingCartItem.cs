using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InitCMS.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string ShoppingCartId { get; set; }
    }
}
