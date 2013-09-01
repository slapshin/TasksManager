using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.SPA.Areas.Admin.Models
{
    public class ProjectDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }

        public string Comment { get; set; }

        [Required(ErrorMessage = "Мастер не выбран")]
        public Guid? Master { get; set; }

        public string MasterName { get; set; }

        public ProjectPriority Priority { get; set; }
    }
}