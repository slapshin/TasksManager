using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Common.Auth;
using Web.SPA.Common;

namespace Web.SPA.Controllers
{
    public class LoginController : BaseController
    {
        [HttpPost]
        public JsonResult JsonLogin(LoginView model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Login(model.Login, model.Password);
                    return Json(new { success = true, redirect = returnUrl });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return Json(new { errors = GetErrorsFromModelState() });
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult Index(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Login(loginView.Login, loginView.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(loginView);
        }

        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        private void Login(string login, string password)
        {
            if (Auth.Login(login, password, true) == null)
            {
                throw new ApplicationException("Пользователь не существует или пароль задан неверно");
            }
        }
    }
}