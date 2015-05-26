using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ClaimView
    {
        public Guid? Id { get; set; }

        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
        public string Title { get; set; }

        public string Comment { get; set; }

        [Required(ErrorMessage = "Укажите заказчика")]
        public Guid Customer_Id { get; set; }

        public string Customer_Login { get; set; }

        public string Customer_Name { get; set; }

        public string Customer_Surname { get; set; }

        public Guid? Project_Id { get; set; }

        public string Project_Title { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskType Type { get; set; }

        public bool InArchive { get; set; }
    }
}