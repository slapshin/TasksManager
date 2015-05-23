using Common;
using Model;
using NHibernate;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Web.Areas.Admin.Models;
using Web.Utils;

namespace Web.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            return View(new PageableData<User>(DbSession, page, null, null));
        }

        public ActionResult Create()
        {
            return View("Edit", new UserView());
        }

        public ActionResult Edit(Guid id)
        {
            return View(ModelMapper.Map<User, UserView>(LoadEntity<User>(id)));
        }

        [HttpPost]
        public ActionResult Edit(UserView view)
        {
            if (ModelState.IsValid)
            {
                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    User user = view.Id.HasValue ? LoadEntity<User>(view.Id.Value) : new User();
                    user = ModelMapper.Map<UserView, User>(view, user);
                    DbSession.SaveOrUpdate(user);
                    trans.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View(view);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordView view)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (ModelError error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        builder.AppendLine(error.ErrorMessage);
                    }
                    throw new ApplicationException(builder.ToString());
                }

                using (ITransaction trans = DbSession.BeginTransaction())
                {
                    GetEntity<User>(view.Id).Password = Helpers.CreateMD5Hash(view.Password);
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
                DbSession.Delete(LoadEntity<User>(id));
                trans.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}
