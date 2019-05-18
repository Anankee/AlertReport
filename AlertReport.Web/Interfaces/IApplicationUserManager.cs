using AlertReport.Web.Infrastructure;
using AlertREport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlertReport.Web.Interfaces
{
    public interface IUserManager : IDisposable
    {
        Task<AccountManageResult> RegisterAsync(User user);
        AccountManageResult ChangePassword(int userId, string oldPassword, string newPassword);
        User GetUserByLogin(string v);
    }
}