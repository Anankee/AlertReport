using AlertReport.Db.Interfaces;
using AlertReport.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlertReport.Web.Infrastructure
{
    public class ApplicationAccountManager : IApplicationAccountManager
    {
        private readonly IUnitOfWork unitOfWork;

        public ApplicationAccountManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task<LoginStatus> LoginAsync(string login, string password, bool rememberMe)
        {
            return Task.Run(() =>
            {


                return LoginStatus.ERROR;
            });
        }
    }


    public enum LoginStatus
    {
        SUCCESS = 0,        
        WRONG_LOGIN = 1,
        WRONG_PASSWORD = 2,        
        ERROR = 3
    }
    
}