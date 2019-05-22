using AlertReport.Db.Interfaces;
using AlertReport.Web.Infrastructure;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AlertReport.Web.Controllers
{
    public class AccountController : Controller
    {
        private IAccountManager accountManager;
        private IUserManager userManager;
        
        public AccountController(IAccountManager accountManager, IUserManager userManager)
        {
            this.accountManager = accountManager;
            this.userManager = userManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = accountManager.Login(model.LoginOrEmail, model.Password, model.RememberMe);
            if (!result.IsSuccess)
            {
                AddErrors(result.Error);
                return View(model);
            }

            if (model.RememberMe)
                Response.Cookies.Add(CookieHelper.CreateRememberCookie(model.LoginOrEmail));

            if (Url.IsLocalUrl(returnUrl))            
                return Redirect(returnUrl);            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await userManager.RegisterAsync(new User(model.Login, model.Password, model.Email, model.PhoneNumber));
            if (result.IsSuccess)
            {
                accountManager.Login(model.Login, model.Password);
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result.Error);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckLoginAvailability(string login)
        {
            var user = userManager.GetUserByLogin(login);
            if (user == null)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            return Json(new { result = false, text = string.Format("User with login {0} already exists!", login) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckEmailAvailability(string email)
        {
            var user = userManager.GetUserByEmail(email);
            if (user == null)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            return Json(new { result = false, text = string.Format("User with email address {0} already exists!", email) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AlertAuthorization]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AlertAuthorization]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = userManager.ChangePassword(model.UserId, model.OldPassword, model.NewPassword);

            if (result.IsSuccess)            
                return RedirectToAction("ChangePasswordConfirmation");

            AddErrors(result.Error);
            return View();
        }

        [HttpGet]
        public ActionResult ChangePasswordConfirmation()
        {
            accountManager.LogOut(Response);
            return View();
        }

        [HttpGet]
        [AlertAuthorization]
        public ActionResult LogOff()
        {
            accountManager.LogOut(Response);
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (accountManager != null)
                {
                    accountManager.Dispose();
                    accountManager = null;
                }
                if(userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private void AddErrors(params string[] error)
        {
            foreach (var e in error)
            {
                ModelState.AddModelError("", e);
            }
        }
        #endregion
    }
}