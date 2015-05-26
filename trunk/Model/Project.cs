using Model.Common;
using System;
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

        public virtual void AddCall(Call call)
        {
            Calls.Add(call);
            call.Project = this;
        }

        public virtual void RemoveCall(Call call)
        {
            Calls.Remove(call);
            call.Project = null;
        }

        public virtual void AddExecutor(User user)
        {
            if (!user.IsExecutor)
            {
                throw new ApplicationException("Пользователь не является исполнителем");
            }
            Executors.Add(user);
        }

        public virtual void RemoverExecutor(User user)
        {
            Executors.Remove(user);
        }

        public virtual void AddObserver(User user)
        {
            if (!user.IsCustomer)
            {
                throw new ApplicationException("Пользователь не является заказчиком");
            }
            Observers.Add(user);
        }

        public virtual void RemoverObserver(User user)
        {
            Observers.Remove(user);
        }
    }
}