using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InitCMS.ViewModel
{
    public class UserEditViewModel
    {

        [Key]
        public int UserId { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
