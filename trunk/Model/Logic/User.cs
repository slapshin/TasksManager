using System;

namespace Model
{
    public partial class User
    {
        public virtual bool IsAdmin
        {
            get { return Roles.HasFlag(UserRole.Admin); }
            set { AddOrRemoveRole(value, UserRole.Admin); }
        }

        public virtual bool IsCustomer
        {
            get { return Roles.HasFlag(UserRole.Customer); }
            set { AddOrRemoveRole(value, UserRole.Customer); }
        }

        public virtual bool IsExecutor
        {
            get { return Roles.HasFlag(UserRole.Executor); }
            set { AddOrRemoveRole(value, UserRole.Executor); }
        }

        public virtual bool IsMaster
        {
            get { return Roles.HasFlag(UserRole.Master); }
            set { AddOrRemoveRole(value, UserRole.Master); }
        }

        public virtual bool IsRouter
        {
            get { return Roles.HasFlag(UserRole.Router); }
            set { AddOrRemoveRole(value, UserRole.Router); }
        }

        public virtual bool IsTester
        {
            get { return Roles.HasFlag(UserRole.Tester); }
            set { AddOrRemoveRole(value, UserRole.Tester); }
        }

        public virtual bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }

            string[] rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string role in rolesArray)
            {
                UserRole tmp;
                if (Enum.TryParse<UserRole>(role, out tmp) && (Roles.HasFlag(tmp)))
                {
                    return true;
                }
            }
            return false;
        }

        private void AddOrRemoveRole(bool value, UserRole role)
        {
            if (value)
            {
                Roles |= role;
            }
            else
            {
                Roles &= ~(role);
            }
        }
    }
}