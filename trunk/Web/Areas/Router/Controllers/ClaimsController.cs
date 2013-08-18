using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Areas.Router.Controllers
{
    public class ClaimsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            return View(new PageableData<Claim>(DbSession, page, null, Filter));
        }

        private void Filter(ICriteria criteria)
        {
            criteria.Add(Restrictions.IsNull("Project"));
        }

        public ActionResult Projects()
        {
            return PartialView(DbSession.QueryOver<Project>().List());
        }

        public ActionResult AssignProject(Guid claim, Guid project)
        {
            try
            {
                using (ITransaction transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Claim>(claim, "Требование не найдено").Project = LoadEntity<Project>(project);
                    transaction.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }
    }
}
