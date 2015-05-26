using Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Models;
using Web.Utils;

namespace Web.Areas.Master.Controllers
{
    public class ProjectsController : BaseController
    {
        public ActionResult Details(Guid id, int page = 1)
        {
            ViewBag.Page = page;
            return View(ModelMapper.Map<Project, ProjectView>(GetEntity<Project>(id)));
        }

        public ActionResult Edit(Guid id)
        {
            return View(ModelMapper.Map<Project, ProjectView>(GetEntity<Project>(id)));
        }

        [HttpPost]
        public ActionResult Edit(ProjectView projectView)
        {
            if (ModelState.IsValid)
            {
                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    var project = GetEntity<Project>(projectView.Id.Value);
                    project = ModelMapper.Map(projectView, project);
                    DbSession.SaveOrUpdate(project);
                    trans.Commit();
                    return RedirectToAction("Edit", new { id = projectView.Id });
                }
            }
            return View(projectView);
        }

        [HttpPost]
        public ActionResult Calls(Guid id)
        {
            try
            {
                List<object> data = new List<object>();
                foreach (Call call in GetEntity<Project>(id, string.Format("Проект {0} не найден", id)).Calls)
                {
                    data.Add(new
                    {
                        id = call.Id,
                        title = call.Title ?? "",
                        status = call.Status.ToString(),
                        created = call.Created
                    });
                }
                return SuccessJson(data);
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        public ActionResult Executors(Guid id)
        {
            ViewBag.ProjectId = id;
            return PartialView(GetEntity<Project>(id).Executors.Select(e => e.Login).ToList());
        }

        public ActionResult ExecutorsManagement(Guid id)
        {
            ViewBag.ProjectId = id;
            return PartialView(GetEntity<Project>(id).Executors);
        }

        public ActionResult AvailableExecutors(Guid id)
        {
            // TO DO плохой запрос
            var project = LoadEntity<Project>(id);
            var users = DbSession.CreateCriteria<User>()
                            .List<User>()
                            .Where(u => u.IsExecutor && !project.Executors.Contains(u))
                            .ToList();
            return PartialView(users);
        }

        [HttpPost]
        public ActionResult RemoveExecutor(Guid id, Guid exec)
        {
            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Project>(id, string.Format("Проект {0} не найден", id)).RemoverExecutor(LoadEntity<User>(exec));
                    transaction.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        [HttpPost]
        public ActionResult AddExecutor(Guid id, Guid exec)
        {
            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Project>(id, "Проект не найден").AddExecutor(GetEntity<User>(exec, "Пользователь не найден"));
                    transaction.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        public ActionResult Observers(Guid id)
        {
            ViewBag.ProjectId = id;
            return PartialView(GetEntity<Project>(id).Observers.Select(e => e.Login).ToList());
        }

        public ActionResult ObserversManagement(Guid id)
        {
            ViewBag.ProjectId = id;
            return PartialView(GetEntity<Project>(id).Observers);
        }

        public ActionResult AvailableObservers(Guid id)
        {
            // TO DO плохой запрос
            Project project = LoadEntity<Project>(id);
            var users = DbSession.CreateCriteria<User>()
                            .List<User>()
                            .Where(u => u.IsCustomer && !project.Observers.Contains(u))
                            .ToList();
            return PartialView(users);
        }

        [HttpPost]
        public ActionResult RemoveObserver(Guid id, Guid observer)
        {
            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Project>(id, string.Format("Проект {0} не найден", id)).RemoverObserver(LoadEntity<User>(observer));
                    transaction.Commit();
                    return SuccessJson();
                }
            }
            catch (Exception e)
            {
                return FailedJson(e.Message);
            }
        }

        [HttpPost]
        public ActionResult AddObserver(Guid id, Guid observer)
        {
            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Project>(id, "Проект не найден").AddObserver(GetEntity<User>(observer, "Пользователь не найден"));
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