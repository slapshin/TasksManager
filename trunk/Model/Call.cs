using Model.Common;
using System;
using System.Collections.Generic;

namespace Model
{
    public partial class Call : Entity<Call>
    {
        #region fields

        private ISet<Task> tasks = new HashSet<Task>();

        #endregion fields

        #region properties

        public virtual DateTime? Checked { get; set; }

        public virtual Claim Claim { get; set; }

        public virtual string Comment { get; set; }

        public virtual DateTime? Completed { get; set; }

        public virtual DateTime? Created { get; set; }

        public virtual int Id { get; set; }

        public virtual bool InArchive { get; set; }

        public virtual TaskPriority Priority { get; set; }

        public virtual Project Project { get; set; }

        public virtual DateTime? Returned { get; set; }

        public virtual CallStatus Status { get; set; }

        public virtual string Title { get; set; }

        #endregion properties

        #region collections

        public virtual ISet<Task> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        #endregion collections

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