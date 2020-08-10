
using System.Collections.Generic;

namespace InitCMS.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Product { get; set; }

    }
}
