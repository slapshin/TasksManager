using Model.Common;
using System;

namespace Model
{
    public class Claim : IdEntity<Claim>
    {
        #region properties
        public virtual User Customer { get; set; }
        public virtual DateTime? Created { get; set; }
        public virtual string Title { get; set; }
        public virtual string Comment { get; set; }
        public virtual TaskPriority Priority { get; set; }
        public virtual Project Project { get; set; }
        public virtual TaskType Type { get; set; }
        public virtual Call Call { get; set; }
        #endregion
    }
}
