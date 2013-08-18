using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models
{
    public class ChangePasswordView
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string ConfirmPassword { get; set; }
    }
}