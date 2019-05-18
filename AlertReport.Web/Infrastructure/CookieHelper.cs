using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AlertREport.Db.Models;

namespace AlertReport.Web.Infrastructure
{
    public class CookieHelper
    {
        private const string LOGIN_COOKIE = "LOGIN_COOKIE";

        internal static HttpCookie CreateRememberCookie(string login)
        {
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
            return new HttpCookie(LOGIN_COOKIE, FormsAuthentication.Encrypt(authTicket));
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
    }
}