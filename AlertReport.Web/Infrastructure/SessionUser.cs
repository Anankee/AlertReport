using AlertREport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlertReport.Web.Infrastructure
{
    public class SessionParser
    {
        public static User User
        {
            get => HttpContext.Current.Session["user"] as User;
            set => HttpContext.Current.Session["user"] = value;
        }
    }
}