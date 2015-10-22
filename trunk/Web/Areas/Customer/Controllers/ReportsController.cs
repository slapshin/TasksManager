using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(DbSession.QueryOver<Project>()
                        .JoinQueryOver<User>(p => p.Observers)
                        .Where(o => o.Id == CurrentUser.Id)
                        .List());
        }

        [HttpPost]
        public ActionResult ProjectsReport(Guid id)
        {
            var project = GetEntity<Project>(id);
            var data = new List<object>();
            foreach (var call in project.Calls.Where(c => !c.InArchive))
            {
                var tasks = new List<object>();
                foreach (var task in call.Tasks.Where(t => !t.InArchive))
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