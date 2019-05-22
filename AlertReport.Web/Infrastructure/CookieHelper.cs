using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AlertReport.Db.Models;

namespace AlertReport.Web.Infrastructure
{
    public class CookieHelper
    {
        private const string LOGIN_COOKIE = "Alert me";

        internal static HttpCookie CreateRememberCookie(string login)
        {
            var expiration = DateTime.Now.AddDays(14);
            //create the authentication ticket
            var authTicket = new FormsAuthenticationTicket(
              1,
              login,
              DateTime.Now,
              DateTime.Now.AddDays(14),  // expiry
              true,  //true to remember
              "",
              FormsAuthentication.FormsCookiePath
            );

            //encrypt the ticket and add it to a cookie
            var cookie =  new HttpCookie(LOGIN_COOKIE, FormsAuthentication.Encrypt(authTicket));
            cookie.Expires = expiration;
            cookie.HttpOnly = true;
            return cookie;
        }

        internal static string GetUserLoginFromCookie(HttpCookieCollection cookies)
        {
            var cookie = cookies.Get(LOGIN_COOKIE);

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
                return null;

            var authTicket = FormsAuthentication.Decrypt(cookie.Value);
            if (authTicket.Expired)
                return null;

            return authTicket.Name;
        }

        internal static HttpCookie RemoveRememberCookie(HttpCookieCollection cookies)
        {
            var cookie = cookies.Get(LOGIN_COOKIE);
            cookie.Expires = DateTime.Today.AddDays(-1);
            return cookie;
        }
    }
}