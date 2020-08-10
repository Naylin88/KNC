using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace InitCMS.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        [MaxLength(20)]
        [DisplayName("Title")]
        public string CatTitle { get; set; }
        [MaxLength(100)]
        [DisplayName("Description")]
        public string CatDescription { get; set; }
        public ICollection<Product> Product { get; set; }

    }
}
