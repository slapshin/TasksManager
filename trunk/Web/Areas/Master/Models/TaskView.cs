using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Master.Models
{
    public class TaskView
    {
        public Guid? Id { get; set; }
        public int Call_Id { get; set; }
        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }
        public string Comment { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public TaskType Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }
        public Guid? Executor_Id { get; set; }
    }
}