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
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await applicationAccountManager.LoginAsync(model.LoginOrEmail, model.Password, model.RememberMe);

            switch (result)
            {
                case LoginStatus.SUCCESS:
                    return View();                  //todo
                case LoginStatus.WRONG_LOGIN:
                    return View();                  //todo
                case LoginStatus.WRONG_PASSWORD:
                    return View();                  //todo
                case LoginStatus.ERROR:
                default:
                    return View(model);             //todo
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            //unitOfWork.UserRepository.Add(new User(model.Login, model.Password, model.Email, model.PhoneNumber));
            return View();
        }
    }
}