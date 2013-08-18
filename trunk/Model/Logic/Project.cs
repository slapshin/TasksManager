using System;
namespace Model
{
    public partial class Project
    {
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
