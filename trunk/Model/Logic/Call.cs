using System;
namespace Model
{
    public partial class Call
    {
        protected override object GetId()
        {
            return Id;
        }

        protected override bool IsNew(object id)
        {
            return Equals(Id, 0);
        }

        public virtual void AddTask(Task task)
        {
            Tasks.Add(task);
            task.Call = this;
        }

        public virtual void SetStatus(CallStatus status)
        {
            Status = status;
            switch (status)
            {
                case CallStatus.Created:
                    if (!Created.HasValue)
                    {
                        Created = DateTime.Now;
                    }
                    break;
                case CallStatus.Completed:
                    Completed = DateTime.Now;
                    break;
                case CallStatus.Returned:
                    Returned = DateTime.Now;
                    break;
                case CallStatus.Checked:
                    Checked = DateTime.Now;
                    break;
                default:
                    break;
            }
        }
    }
}
