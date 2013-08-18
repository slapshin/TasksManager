using Model;
using Model.Common;
using NHibernate;
using Ninject;
using System;
using System.Web.Mvc;
using Web.Common.Auth;
using Web.Common.Mapper;
using Web.Common.Repository;

namespace Web.SPA.Common
{
    public abstract class BaseController : Controller
    {
        private ISession dbSession;

        public BaseController()
            : base()
        {
        }

        [Inject]
        public ISessionProvider Provider { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        [Inject]
        public IMapper ModelMapper { get; set; }

        public ISession DbSession
        {
            get
            {
                if (dbSession == null)
                {
                    dbSession = Provider.OpenSession();
                };
                return dbSession;
            }
        }

        public User CurrentUser
        {
            get { return ((IUserProvider)Auth.CurrentUser.Identity).User; }
        }

        protected JsonResult SuccessJson(object data = null)
        {
            return data == null ? Json(new { success = true }) : Json(new { success = true, data = data });
        }

        protected JsonResult FailedJson(string msg)
        {
            return Json(new { success = false, error = msg });
        }

        protected T GetEntity<T>(object id, string errorMsg = "") where T : Entity<T>
        {
            T obj = DbSession.Get<T>(id);
            if (obj == null)
            {
                throw new ApplicationException(string.IsNullOrWhiteSpace(errorMsg) ? string.Format("Объект [id: {0}] не найден", id) : errorMsg);
            }
            return obj;
        }

        protected T LoadEntity<T>(object id) where T : Entity<T>
        {
            return DbSession.Get<T>(id);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = FailedJson(filterContext.Exception.Message);
                filterContext.ExceptionHandled = true;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.CurrentUser = CurrentUser;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            if (dbSession != null && dbSession.IsConnected)
            {
                dbSession.Dispose();
            }
        }
    }
}