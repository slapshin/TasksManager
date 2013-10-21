using System.ComponentModel.DataAnnotations;

namespace Web.Common.Auth
{
    public class LoginView
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}