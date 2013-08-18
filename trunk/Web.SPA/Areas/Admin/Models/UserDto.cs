using System;

namespace Web.SPA.Areas.Admin.Models
{
    public class UserDto
    {
        public Guid? Id { get; set; }

        public string Email { get; set; }

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