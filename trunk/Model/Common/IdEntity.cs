using System;

namespace Model.Common
{
    public abstract class IdEntity<T> : Entity<T> where T : IdEntity<T>
    {
        public virtual Guid Id { get; private set; }

        protected override object GetId()
        {
            return Id;
        }

        protected override bool IsNew(object id)
        {
            return Equals(Id, Guid.Empty);
        }
    }
}
