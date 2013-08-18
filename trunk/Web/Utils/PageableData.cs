using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace Web.Utils
{
    public class PageableData<T> where T : class
    {
        private const int ITEM_PER_PAGE_DEFAULT = 30;

        public PageableData(ISession session, int page, Action<ICriteria> order, Action<ICriteria> filter, int itemPerPage = 0)
        {
            ItemPerPage = itemPerPage == 0 ? ITEM_PER_PAGE_DEFAULT : itemPerPage;
            PageNo = page;

            CalculatePageCount(session, filter);
            FillList(session, order, filter);
        }

        public IEnumerable<T> List { get; set; }

        public int PageNo { get; set; }

        public int PageCount { get; set; }

        public int ItemPerPage { get; set; }

        private void CalculatePageCount(ISession session, Action<ICriteria> filter)
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (filter != null)
            {
                filter(criteria);
            };
            int count = criteria.SetProjection(Projections.Count(Projections.Id()))
                                .UniqueResult<int>();

            PageCount = (int)decimal.Remainder(count, ItemPerPage) == 0 ? count / ItemPerPage : count / ItemPerPage + 1;
        }

        private void FillList(ISession session, Action<ICriteria> order, Action<ICriteria> filter)
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (order != null)
            {
                order(criteria);
            }

            if (filter != null)
            {
                filter(criteria);
            };

            List = criteria.SetFirstResult(ItemPerPage * (PageNo - 1))
                            .SetMaxResults(ItemPerPage).List<T>();
        }
    }
}