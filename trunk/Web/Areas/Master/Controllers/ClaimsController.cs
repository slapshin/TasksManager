using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Areas.Master.Controllers
{
    public class ClaimsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            return View(new PageableData<Claim>(DbSession, page, null, ClaimFilter));
        }

        private void ClaimFilter(ICriteria criteria)
        {
            criteria.CreateAlias("Project", "project")
                    .Add(Expression.Eq("project.Master", CurrentUser))
                    .Add(Expression.Eq("InArchive", false));
        }

        [HttpPost]
        public ActionResult CreateCall(Guid id)
        {
            try
            {
                Claim claim = GetEntity<Claim>(id, "Требование не найдено");
                using (ITransaction transaction = DbSession.BeginTransaction())
                {
                    Call call = new Call()
                    {
                        Created = DateTime.Now,
                        Title = claim.Title,
                        Comment = claim.Comment,
                        Claim = claim,
                        Project = claim.Project
                    };
                    claim.Call = call;
                    DbSession.Save(call);
                    transaction.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        public ActionResult Delete(Guid id)
        {
            using (ITransaction trans = DbSession.BeginTransaction())
            {
                GetEntity<Claim>(id).InArchive = true;
                trans.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}