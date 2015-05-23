using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Master.Models
{
    public class CallView
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }

        public string Comment { get; set; }

        [Required(ErrorMessage = "Назначьте проект")]
        public Guid Project_Id { get; set; }

        public string Project_Title { get; set; }

        public DateTime Created { get; set; }

        public TaskPriority Priority { get; set; }

        public CallStatus Status { get; set; }

        public Guid? Claim_Id { get; set; }

        public bool InArchive { get; set; }
    }
}