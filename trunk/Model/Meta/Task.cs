using Iesi.Collections.Generic;
using Model.Common;
using System;

namespace Model
{
    public partial class Task : IdEntity<Task>
    {
        #region fields

        private ISet<File> files = new HashedSet<File>();

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
    }
}