using System;

namespace Model
{
    public partial class Task
    {
        public virtual void SetStatus(TaskStatus status)
        {
            this.Status = status;
            switch (status)
            {
                case TaskStatus.Created:
                    if (!Created.HasValue)
                    {
                        Created = DateTime.Now;
                    }
                    break;
                case TaskStatus.Returned:
                    Returned = DateTime.Now;
                    break;
                case TaskStatus.Completed:
                    Completed = DateTime.Now;
                    break;
                case TaskStatus.Executing:
                    if (!Executing.HasValue)
                    {
                        Executing = DateTime.Now;
                    };
                    break;
                case TaskStatus.Checked:
                    Checked = DateTime.Now;
                    break;
                default:
                    break;
            }
        }
    }
}
