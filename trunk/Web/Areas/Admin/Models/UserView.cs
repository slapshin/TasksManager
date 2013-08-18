using DataAnnotationsExtensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models
{
    public class UserView
    {
        public Guid? Id { get; set; }

        [Email]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsMaster { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsExecutor { get; set; }

        public bool IsRouter { get; set; }

        public bool IsTester { get; set; }
    }
}