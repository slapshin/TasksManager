using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Web.Mvc;
using Web.Models;
using Web.Utils;

namespace Web.Areas.Customer.Controllers
{
    public class ClaimsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            return View(new PageableData<Claim>(DbSession, page, null, Filter));
        }

        private void Filter(ICriteria criteria)
        {
            criteria.Add(Restrictions.Eq("Customer", CurrentUser));
        }

        public ActionResult Create()
        {
            return View("Edit", new ClaimView());
        }

        public ActionResult Edit(Guid id)
        {
            return View(ModelMapper.Map<Claim, ClaimView>(GetEntity<Claim>(id)));
        }

        [HttpPost]
        public ActionResult Edit(ClaimView claimView)
        {
            if (ModelState.IsValid)
            {
                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    Claim claim = null;
                    if (claimView.Id.HasValue)
                    {
                        claim = GetEntity<Claim>(claimView.Id.Value);
                    }
                    else
                    {
                        claim = new Claim()
                        {
                            Customer = CurrentUser,
                            Created = DateTime.Now
                        };
                    }

                    claim = ModelMapper.Map<ClaimView, Claim>(claimView, claim);
                    DbSession.SaveOrUpdate(claim);
                    trans.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View(claimView);
        }

        [HttpPost]
        public ActionResult ReturnCall(int id, string comment)
        {
            try
            {
                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    Call call = GetEntity<Call>(id);
                    call.SetStatus(CallStatus.Returned);
                    call.Comment += Environment.NewLine + comment;
                    trans.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        [HttpPost]
        public ActionResult SetCallChecked(int id)
        {
            try
            {
                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    GetEntity<Call>(id).SetStatus(CallStatus.Checked);
                    trans.Commit();
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
                DbSession.Delete(LoadEntity<Claim>(id));
                trans.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}
