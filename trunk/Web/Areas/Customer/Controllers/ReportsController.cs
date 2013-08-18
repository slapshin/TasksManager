using Model;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Areas.Customer.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProjectsReport()
        {
            IList<Project> projects = DbSession.CreateCriteria<Project>()
                .CreateCriteria("Observers")
                .Add(Expression.Eq("Id", CurrentUser.Id))
                .List<Project>();

            return View(projects);
        }

        [HttpPost]
        public ActionResult ProjectsReport(Guid id)
        {
            Project project = GetEntity<Project>(id);
            IList<object> data = new List<object>();
            foreach (Call call in project.Calls)
            {
                IList<object> tasks = new List<object>();
                foreach (Task task in call.Tasks)
                {
                    tasks.Add(new
                    {
                        title = task.Title ?? "",
                        status = task.Status.ToString(),
                        created = task.Created,
                        exec = task.Executor != null ? task.Executor.Login : ""
                    });
                }

                data.Add(new
                {
                    id = call.Id,
                    title = call.Title ?? "",
                    status = call.Status.ToString(),
                    created = call.Created,
                    tasks = tasks
                });
            }
            return SuccessJson(data);
        }

        public ActionResult ExecutorsReport()
        {
            if (Request.IsAjaxRequest())
            {

            }
            return View();
        }
    }
}
