using System.ComponentModel.DataAnnotations;

namespace MB.TestTask.WebAPI.Models
{
    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}