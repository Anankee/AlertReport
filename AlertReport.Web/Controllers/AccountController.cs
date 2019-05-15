using AlertReport.Db.Interfaces;
using AlertReport.Web.Infrastructure;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using AlertREport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationAccountManager applicationAccountManager;
        
        public AccountController(IApplicationAccountManager applicationAccountManager)
        {
            this.applicationAccountManager = applicationAccountManager;
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

            var result = applicationAccountManager.Login(model.LoginOrEmail, model.Password, model.RememberMe);
            if (result.IsSuccess)
                return RedirectToAction("Index", "Home");

            AddErrors(result.Errors.ToArray());
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await applicationAccountManager.RegisterAsync(new User(model.Login, model.Password, model.Email, model.PhoneNumber));
            if (result.IsSuccess)
            {
                applicationAccountManager.Login(model.Login, model.Password);
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result.Errors.ToArray());
            return View();
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