using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Areas.Executor.Controllers
{
    public class TasksController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            return View(new PageableData<Task>(DbSession, page, null, filter));
        }

        private void filter(ICriteria criteria)
        {
            criteria.Add(Expression.Eq("Executor", CurrentUser));
            criteria.Add(Expression.Not(Expression.In("Status", new[] { TaskStatus.Completed, TaskStatus.Checked })));
        }

        public ActionResult Kanban()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Tasks()
        {
            try
            {
                IList<Task> tasks = DbSession.CreateCriteria<Task>()
                                       .Add(Expression.Eq("Executor", CurrentUser))
                                       .List<Task>();

                List<object> result = new List<object>();
                foreach (Task task in tasks)
                {
                    result.Add(new
                    {
                        id = task.Id,
                        title = task.Title ?? "",
                        status = task.Status.ToString(),
                        project = (task.Call != null) && (task.Call.Project != null) ? task.Call.Project.Title : ""
                    });
                }
                return Json(new { success = true, data = result });
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        [HttpPost]
        public ActionResult SetStatus(Guid id, TaskStatus status)
        {
            try
            {
                using (ITransaction transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Task>(id, string.Format("Задание {0} не найдено", id)).SetStatus(status);
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
