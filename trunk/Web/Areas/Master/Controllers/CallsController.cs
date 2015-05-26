using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Areas.Master.Models;
using Web.Utils;

namespace Web.Areas.Master.Controllers
{
    public class CallsController : BaseController
    {
        public ActionResult Index(Guid project, int page = 1)
        {
            ViewBag.ProjectId = project;
            return PartialView(new PageableData<Call>(DbSession,
                                                page,
                                                criteria => criteria.AddOrder(Order.Desc("Created")),
                                                criteria => criteria.Add(Expression.Eq("Project", DbSession.Load<Project>(project)))));
        }

        public ActionResult Kanban(Guid project)
        {
            return View(GetEntity<Project>(project));
        }

        [HttpPost]
        public ActionResult Tasks(int id)
        {
            try
            {
                List<object> result = new List<object>();
                foreach (Task task in GetEntity<Call>(id).Tasks)
                {
                    result.Add(new
                    {
                        id = task.Id,
                        title = task.Title ?? "",
                        status = task.Status.ToString(),
                        exec = task.Executor == null ? "" : task.Executor.Login,
                    });
                }
                return Json(new { success = true, data = result });
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        public ActionResult Create(Guid project)
        {
            Project p = GetEntity<Project>(project);

            return View("Edit", new CallView()
            {
                Project_Id = p.Id,
                Project_Title = p.Title
            });
        }

        public ActionResult Edit(int id)
        {
            ViewBag.CurrentUser = CurrentUser.Id;
            return View(ModelMapper.Map<Call, CallView>(GetEntity<Call>(id)));
        }

        [HttpPost]
        public ActionResult Edit(CallView view)
        {
            if (ModelState.IsValid)
            {
                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    Call call = null;
                    if (view.Id.HasValue)
                    {
                        call = LoadEntity<Call>(view.Id.Value);
                    }
                    else
                    {
                        call = new Call();
                        call.SetStatus(CallStatus.Created);
                    }

                    call = ModelMapper.Map<CallView, Call>(view, call);
                    call.SetStatus(view.Status);
                    ChangeProject(call, view.Project_Id);
                    DbSession.SaveOrUpdate(call);
                    trans.Commit();
                    return RedirectToAction("Details", "Projects", new { id = view.Project_Id });
                }
            }
            return View(view);
        }

        [HttpPost]
        public ActionResult SetStatus(int id, CallStatus status)
        {
            try
            {
                using (ITransaction transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Call>(id).SetStatus(status);
                    transaction.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        private void ChangeProject(Call call, Guid project)
        {
            if ((call.Project != null) && (call.Project.Id == project))
            {
                return;
            }

            if (call.Project != null)
            {
                call.Project.RemoveCall(call);
            }

            DbSession.Load<Project>(project).AddCall(call);
        }

        public ActionResult Delete(int id)
        {
            using (ITransaction trans = DbSession.BeginTransaction())
            {
                Call call = LoadEntity<Call>(id);
                if (call.Claim != null)
                {
                    call.Claim.Call = null;
                }
                Guid project = call.Project.Id;
                DbSession.Delete(call);
                trans.Commit();
                return RedirectToAction("Details", "Projects", new { id = project });
            }
        }

        public ActionResult Details(Guid id)
        {
            return View();
        }
    }
}