using AlertReport.Db.Interfaces;
using AlertReport.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlertReport.Web.Interfaces
{
    public interface IApplicationAccountManager
    {
        Task<LoginStatus> LoginAsync(string login, string password, bool rememberMe);
    }
}