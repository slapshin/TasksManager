using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Common.Repository;

namespace Web.Utils
{
    public static class ViewUtils
    {
        private static IdSelectListItem[] EMPTY_ID_SELECT_LIST_ITEM = new[] { new IdSelectListItem() };

        private static ISessionProvider SessionProvider
        {
            get { return DependencyResolver.Current.GetService<ISessionProvider>(); }
        }

        public static SelectList UserSelectList(UserRole role, Guid? selected)
        {
            //TODO плохой запрос: приходится выбирать всех пользователей, а уже потом фильтровать
            return CreateIdSelectList(selected, (ISession session) =>
            {
                return session.CreateCriteria<User>()
                            .List<User>()
                            .Where(u => u.Roles.HasFlag(role))
                            .Select(u => new IdSelectListItem()
                            {
                                Id = u.Id,
                                Text = u.Login
                            });
            });
        }

        public static SelectList ProjectsSelectList(Guid? selected)
        {
            return CreateIdSelectList(selected, (ISession session) =>
            {
                return session.CreateCriteria<Project>()
                            .List<Project>()
                            .Select(u => new IdSelectListItem
                            {
                                Id = u.Id,
                                Text = u.Title
                            });
            });
        }

        public static SelectList ProjectsSelectListOfMaster(Guid? selected, Guid master)
        {
            return CreateIdSelectList(selected, (ISession session) =>
                {
                    return session.CreateCriteria<Project>()
                                .Add(Restrictions.Eq("Master", session.Load<User>(master)))
                                .List<Project>()
                                .Select(u => new IdSelectListItem
                                {
                                    Id = u.Id,
                                    Text = u.Title
                                });
                });
        }

        public static SelectList ExecutorsSelectList(ICollection<User> execs, Guid? selected)
        {
            return CreateIdSelectList(selected, (ISession session) =>
            {
                return execs.Select(e => new IdSelectListItem
                                            {
                                                Id = e.Id,
                                                Text = e.Login
                                            });
            });
        }

        private static SelectList CreateIdSelectList(Guid? selected, Func<ISession, IEnumerable<IdSelectListItem>> fillItems)
        {
            using (ISession session = SessionProvider.OpenSession())
            {
                return new SelectList(EMPTY_ID_SELECT_LIST_ITEM.Concat(fillItems(session)), "Id", "Text", selected);
            }
        }

        public static int ClaimsWithoutProjectsCount()
        {
            using (ISession session = SessionProvider.OpenSession())
            {
                return session.CreateCriteria<Claim>()
                         .Add(Restrictions.IsNull("Project"))
                         .SetProjection(Projections.Count(Projections.Id()))
                         .UniqueResult<int>();
            }
        }

        public static int ClaimsOfMasterWithoutCalls(User master)
        {
            using (ISession session = SessionProvider.OpenSession())
            {
                return session.CreateCriteria<Claim>()
                            .CreateAlias("Project", "project")
                            .Add(Expression.Eq("project.Master", master))
                            .Add(Restrictions.IsNull("Call"))
                            .SetProjection(Projections.Count(Projections.Id()))
                            .UniqueResult<int>();
            }
        }
    }
}