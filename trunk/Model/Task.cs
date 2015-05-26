using Model.Common;
using System;
using System.Collections.Generic;

namespace Model
{
    public partial class Task : IdEntity<Task>
    {
        #region fields

        private ISet<File> files = new HashSet<File>();

        #endregion fields

        #region properties

        public virtual Call Call { get; set; }

        public virtual DateTime? Checked { get; set; }

        public virtual string Comment { get; set; }

        public virtual DateTime? Completed { get; set; }

        public virtual DateTime? Created { get; set; }

        public virtual DateTime? Deadline { get; set; }

        public virtual DateTime? Executing { get; set; }

        public virtual User Executor { get; set; }

        public virtual bool InArchive { get; set; }

        public virtual TaskPriority Priority { get; set; }

        public virtual DateTime? Returned { get; set; }

        public virtual TaskStatus Status { get; set; }

        public virtual string Title { get; set; }

        public virtual TaskType Type { get; set; }

        #endregion properties

        #region collections

        public virtual ISet<File> Files
        {
            get { return files; }
            set { files = value; }
        }

        #endregion collections

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