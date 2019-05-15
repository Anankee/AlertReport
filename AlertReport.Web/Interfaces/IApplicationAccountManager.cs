using AlertReport.Db.Interfaces;
using AlertReport.Web.Infrastructure;
using AlertREport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlertReport.Web.Interfaces
{
    public interface IApplicationAccountManager
    {
        AccountManageResult Login(string login, string password, bool rememberMe = false);
        Task<AccountManageResult> RegisterAsync(User user);
    }
}