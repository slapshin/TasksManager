using System;

namespace Web.Models
{
    public class CustomerView
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}