using Model;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Common.Repository;
using Web.SPA.Models;

namespace Web.SPA.Common
{
    public static class Utils
    {
        private static ISessionProvider SessionProvider
        {
            get { return DependencyResolver.Current.GetService<ISessionProvider>(); }
        }

        public static IEnumerable<ComboBoxDto> UserComboBoxItems(UserRole role)
        {
            using (ISession session = SessionProvider.OpenSession())
            {
                return session.CreateCriteria<User>()
                            .List<User>()
                            .Where(u => u.Roles.HasFlag(role))
                            .Select(u => new ComboBoxDto()
                            {
                                Value = u.Id,
                                Display = u.Login
                            });
            }
        }
    }
}