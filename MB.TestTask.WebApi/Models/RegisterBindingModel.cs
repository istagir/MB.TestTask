using System.ComponentModel.DataAnnotations;

namespace MB.TestTask.WebAPI.Models
{
    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "UserName")]
        [StringLength(100, ErrorMessage = "Максимальная длина имени пользователя 100 символов.")]
        [RegularExpression(@"^[А-я]{3,}$",
            ErrorMessage = "Имя пользователя может содержать только кириллицу и иметь длину не менее 3 символов.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Login")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            ErrorMessage = "Введённое значение не является корректным e-mail адресом.")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"(?=.*\d)((?=.*[A-Z])|(?=.*[А-Я]))((?=.*[a-z])|(?=.*[а-я])).{6,}",
            ErrorMessage = "Пароль должен быть не менее 6 символов, содержать как минимум 1 цифру, 1 прописную и 1 заглавную букву.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Введённые пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}