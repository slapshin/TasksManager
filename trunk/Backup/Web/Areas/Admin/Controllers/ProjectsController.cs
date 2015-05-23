using Model;
using NHibernate;
using System;
using System.Web.Mvc;
using Web.Models;
using Web.Utils;

namespace Web.Areas.Admin.Controllers
{
    public class ProjectsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            return View(new PageableData<Project>(DbSession, page, null, null));
        }

        public ActionResult Create()
        {
            return View("Edit", new ProjectView());
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
                    Project project = projectView.Id.HasValue ? GetEntity<Project>(projectView.Id.Value) : new Project();
                    project = ModelMapper.Map<ProjectView, Project>(projectView, project);
                    project.Master = DbSession.Load<User>(projectView.Master);
                    DbSession.SaveOrUpdate(project);
                    trans.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View(projectView);
        }

        public ActionResult Delete(Guid id)
        {
            using (ITransaction trans = DbSession.BeginTransaction())
            {
                DbSession.Delete(LoadEntity<Project>(id));
                trans.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}
