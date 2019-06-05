using AlertReport.Db.Interfaces;
using AlertReport.Web.Infrastructure;
using AlertReport.Web.Models;
using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlertReport.Web.Interfaces
{
    public interface IAccountManager
    {
        AccountManageResult Login(string login, string password, bool rememberMe = false);
        void LogOut(HttpResponseBase response);
        void Dispose();
    }
}