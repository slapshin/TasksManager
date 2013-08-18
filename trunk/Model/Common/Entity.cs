
namespace Model.Common
{
    public abstract class Entity<T> where T : Entity<T>
    {
        private int? hashCodeCache;

        protected abstract object GetId();
        protected abstract bool IsNew(object id);

        public override bool Equals(object obj)
        {
            var other = obj as T;

            if (other == null)
            {
                return false;
            }
            object thisId = GetId();
            object otherId = other.GetId();

            bool thisIsNew = IsNew(thisId);
            bool otherIsNew = IsNew(otherId);

            if (thisIsNew && otherIsNew)
            {
                return ReferenceEquals(this, other);
            }
            return thisId.Equals(otherId);
        }

        public override int GetHashCode()
        {
            if (hashCodeCache.HasValue)
            {
                return hashCodeCache.Value;
            }

            object id = GetId();
            if (IsNew(id))
            {
                return base.GetHashCode();
            }

            hashCodeCache = id.GetHashCode();

            return id.GetHashCode();
        }

        public static bool operator ==(Entity<T> lhs, Entity<T> rhs)
        {
            return Equals(lhs, rhs);
        }
        public static bool operator !=(Entity<T> lhs, Entity<T> rhs)
        {
            return !Equals(lhs, rhs);
        }
    }
}
