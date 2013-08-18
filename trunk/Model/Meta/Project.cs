using Iesi.Collections.Generic;
using Model.Common;

namespace Model
{
    public partial class Project : IdEntity<Project>
    {
        #region fields

        private ISet<Call> calls = new HashedSet<Call>();
        private ISet<Claim> claims = new HashedSet<Claim>();
        private ISet<User> executors = new HashedSet<User>();
        private ISet<User> observers = new HashedSet<User>();

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