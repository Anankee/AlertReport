using AlertReport.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Infrastructure
{
    public class AlertAuthorizationAttribute : AuthorizeAttribute
    {
        private IUserManager userManager;

        public AlertAuthorizationAttribute()
        {
            userManager = DependencyResolver.Current.GetService<IUserManager>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = SessionParser.User ?? userManager.GetUserByLogin(CookieHelper.GetUserLoginFromCookie(filterContext.HttpContext.Request.Cookies));


            if (user == null)
            {
                var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var action = filterContext.ActionDescriptor.ActionName;
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = string.Format("/{0}/{1}", controller, action) }));
            }
                

            //base.OnAuthorization(filterContext);
        }
    }
}