using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Web.Mvc;
using Web.Areas.Master.Models;
using Web.Utils;

namespace Web.Areas.Master.Controllers
{
    public class TasksController : BaseController
    {
        private static TaskStatus[] ActiveTaskStatus = { TaskStatus.Created, TaskStatus.Executing, TaskStatus.Returned };
        private static TaskStatus[] CompletedTaskStatus = { TaskStatus.Checked, TaskStatus.Completed };

        public ActionResult Index(int call, ShowedTasksType showedTasks = ShowedTasksType.All, int page = 1)
        {
            var obj = GetEntity<Call>(call);
            ViewBag.ShowedTasksType = showedTasks;
            ViewBag.CallId = obj.Id;
            ViewBag.ProjectId = obj.Project.Id;
            return View(new PageableData<Task>(DbSession, page, null, criteria =>
            {
                criteria.Add(Expression.Eq("Call", obj));
                TaskStatus[] statuses = GetStatusesForShow(showedTasks);
                if (statuses.Length > 0)
                {
                    criteria.Add(Expression.In("Status", statuses));
                }
            }));
        }

        public ActionResult Kanban(int call)
        {
            return View(GetEntity<Call>(call));
        }

        public ActionResult Create(int call)
        {
            var obj = GetEntity<Call>(call);
            ViewBag.Executors = obj.Project.Executors;
            return View("Edit", new TaskView()
                {
                    Call_Id = obj.Id
                });
        }

        public ActionResult Edit(Guid id)
        {
            var task = GetEntity<Task>(id);
            ViewBag.Executors = task.Call.Project.Executors;
            return View(ModelMapper.Map<Task, TaskView>(task));
        }

        [HttpPost]
        public ActionResult Edit(TaskView view)
        {
            if (ModelState.IsValid)
            {
                using (var trans = DbSession.BeginTransaction())
                {
                    Task task = null;
                    if (view.Id.HasValue)
                    {
                        task = LoadEntity<Task>(view.Id.Value);
                    }
                    else
                    {
                        task = new Task();
                        task.SetStatus(TaskStatus.Created);
                        GetEntity<Call>(view.Call_Id).AddTask(task);
                    }

                    task = ModelMapper.Map<TaskView, Task>(view, task);
                    task.SetStatus(view.Status);

                    if (view.Executor_Id.HasValue)
                    {
                        task.Executor = LoadEntity<User>(view.Executor_Id.Value);
                    }

                    DbSession.SaveOrUpdate(task);
                    trans.Commit();

                    ShowedTasksType showedTasks = task.Status == TaskStatus.Completed || task.Status == TaskStatus.Checked ? ShowedTasksType.Completed : ShowedTasksType.Active;
                    return RedirectToAction("Index", "Tasks", new { call = view.Call_Id, showedTasks = showedTasks });
                }
            }
            return View(view);
        }

        [HttpPost]
        public ActionResult SetStatus(Guid id, TaskStatus status)
        {
            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    GetEntity<Task>(id).SetStatus(status);
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
                Task task = LoadEntity<Task>(id);
                int call = task.Call.Id;
                DbSession.Delete(task);
                trans.Commit();
                return RedirectToAction("Index", new { call = call });
            }
        }

        private TaskStatus[] GetStatusesForShow(ShowedTasksType showedTasks)
        {
            switch (showedTasks)
            {
                case ShowedTasksType.Active:
                    return ActiveTaskStatus;

                case ShowedTasksType.Completed:
                    return CompletedTaskStatus;

                default:
                    return new TaskStatus[] { };
            }
        }
    }
}