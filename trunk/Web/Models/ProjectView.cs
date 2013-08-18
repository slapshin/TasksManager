using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ProjectView
    {
        public string Comment { get; set; }

        public Guid? Id { get; set; }

        public Guid? Master { get; set; }

        public ProjectPriority Priority { get; set; }

        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }
    }
}