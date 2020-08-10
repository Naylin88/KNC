
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InitCMS.ViewModel
{
    public class ProductEditViewModel : ProductViewModel
    {
        public new int Id { get; set; }
        [Required]
        [DisplayName("Code")]
      //  [Remote(action: "CheckPCode", controller: "Products")]
        public new string PCode { get; set; }
        public string PhtotPath { get; set; }
    }
}
