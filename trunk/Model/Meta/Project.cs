using Model.Common;
using System.Collections.Generic;

namespace Model
{
    public partial class Project : IdEntity<Project>
    {
        #region fields

        private ISet<Call> calls = new HashSet<Call>();
        private ISet<Claim> claims = new HashSet<Claim>();
        private ISet<User> executors = new HashSet<User>();
        private ISet<User> observers = new HashSet<User>();

        #endregion fields

        #region properties

        public virtual string Comment { get; set; }

        public virtual User Master { get; set; }

        public virtual ProjectPriority Priority { get; set; }

        public virtual string Title { get; set; }

        #endregion properties

        #region collections

        public virtual ISet<Call> Calls
        {
            get { return calls; }
            set { calls = value; }
        }

        public virtual ISet<Claim> Claims
        {
            get { return claims; }
            set { claims = value; }
        }

        public virtual ISet<User> Executors
        {
            get { return executors; }
            set { executors = value; }
        }

        public virtual ISet<User> Observers
        {
            get { return observers; }
            set { observers = value; }
        }

        #endregion collections
    }
}