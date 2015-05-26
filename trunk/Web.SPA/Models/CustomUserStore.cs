using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Common.Repository;

namespace Web.SPA.Models
{
    public class CustomUserStore<TUser> : IUserLoginStore<TUser>,
                                            IUserClaimStore<TUser>,
                                            IUserRoleStore<TUser>,
                                            IUserPasswordStore<TUser>,
                                            IUserSecurityStampStore<TUser>,
                                            IUserStore<TUser>,
                                            IDisposable
                                                where TUser : AuthUser
    {
        private ISessionProvider Provider
        {
            get { return DependencyResolver.Current.GetService<ISessionProvider>(); }
        }

        public CustomUserStore()
        {
        }

        public Task CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return Task.Run(() =>
            {
                AuthUser result = null;
                using (ISession session = Provider.OpenSession())
                {
                    Model.User user = session.Get<Model.User>(Guid.Parse(userId));
                    result = new AuthUser()
                    {
                        Id = user.Id.ToString(),
                        UserName = user.Login,
                        Roles = user.Roles
                    };

                    return result as TUser;
                }
            });
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                AuthUser result = null;
                using (ISession session = Provider.OpenSession())
                {
                    Model.User user = session.QueryOver<Model.User>().Where(u => u.Login == userName).SingleOrDefault();
                    if (user != null)
                    {
                        result = new AuthUser()
                        {
                            Id = user.Id.ToString(),
                            UserName = userName,
                            Roles = user.Roles
                        };
                    }
                }

                return result as TUser;
            });
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            return Task.Run(() =>
            {
                return new List<Claim>()
                {
                    //new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(ClaimTypes.Role, user.Roles.ToString(), "int")
                } as IList<Claim>;
            });
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.Run(() =>
            {
                IList<string> result = new List<string>();
                AddRoleIfExists(user, Model.UserRole.Admin, result);
                AddRoleIfExists(user, Model.UserRole.Customer, result);
                AddRoleIfExists(user, Model.UserRole.Executor, result);
                AddRoleIfExists(user, Model.UserRole.Master, result);
                AddRoleIfExists(user, Model.UserRole.Router, result);
                AddRoleIfExists(user, Model.UserRole.Tester, result);
                return result;
            });
        }

        private void AddRoleIfExists(TUser user, Model.UserRole role, IList<string> roles)
        {
            if (user.Roles.HasFlag(role))
            {
                roles.Add(role.ToString());
            }
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.Run(() =>
            {
                using (ISession session = Provider.OpenSession())
                {
                    return session.Get<Model.User>(Guid.Parse(user.Id)).Password;
                }
            });
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}