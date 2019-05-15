using AlertReport.Db.Interfaces;
using AlertReport.Web.Interfaces;
using AlertREport.Db.Models;
using Microsoft.AspNet.Identity;
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
        private IPasswordHasher passwordHasher;

        public ApplicationAccountManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
        }

        public AccountManageResult Login(string login, string password, bool rememberMe)
        {
            try
            {
                var errors = new List<string>();

                var usersByLogin = unitOfWork.UserRepository.Get(e => e.Login == login);
                if (usersByLogin.Count() == 0)
                    errors.Add("Wrong login");

                var user = usersByLogin.SingleOrDefault(e => passwordHasher.VerifyHashedPassword(e.Password, password) == PasswordVerificationResult.Success);
                if (user == null)
                    errors.Add("Wrong password");


                if (errors.Count > 0)
                    return new AccountManageResult { Errors = errors, IsSuccess = false };

                SessionParser.User = user;
                if (rememberMe)
                {
                    //ToDo
                }

                return new AccountManageResult { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new AccountManageResult { IsSuccess = false, Errors = new List<string> { e.Message } };
            }
        }

        public Task<AccountManageResult> RegisterAsync(User user)
        {
            return Task.Run(() =>
            {
                try
                {
                    user.Password = passwordHasher.HashPassword(user.Password);
                    unitOfWork.UserRepository.Add(user);
                    return new AccountManageResult { IsSuccess = true };
                }
                catch (Exception e)
                {
                    return new AccountManageResult { IsSuccess = false, Errors = new List<string> { e.Message } };
                }
            });
        }
    }

    public class AccountManageResult
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public enum LoginStatus
    {
        SUCCESS = 0,
        WRONG_LOGIN = 1,
        WRONG_PASSWORD = 2,
        ERROR = 3
    }
}