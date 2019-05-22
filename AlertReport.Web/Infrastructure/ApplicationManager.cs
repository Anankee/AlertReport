using AlertReport.Db.Interfaces;
using AlertReport.Db.Models;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace AlertReport.Web.Infrastructure
{
    public class AccountManager : IAccountManager
    {
        private IUnitOfWork unitOfWork;
        private readonly IPasswordHasher passwordHasher;

        public AccountManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
        }

        public AccountManageResult Login(string loginOrEmail, string password, bool rememberMe)
        {
            try
            {
                //Get user by login or email and match password
                var user = GetUserFromDb(loginOrEmail, password);

                if (user == null)
                    return new AccountManageResult { IsSuccess = false, Error = "Wrong login/email or password." };

                SessionParser.User = user;
                return new AccountManageResult { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new AccountManageResult { IsSuccess = false, Error = e.Message };
            }
        }

        public void LogOut(HttpResponseBase response)
        {
            SessionParser.User = null;
            response.Cookies.Add(CookieHelper.RemoveRememberCookie(response.Cookies));
        }

        public void Dispose()
        {
            if (unitOfWork != null)
            {
                unitOfWork.Dispose();
                unitOfWork = null;
            }
        }

        private User GetUserFromDb(string loginOrEmail, string password)
        {
            //First find user by login or email address
            var user = unitOfWork.UserRepository.Get(e => e.Login == loginOrEmail || e.Email == loginOrEmail).SingleOrDefault();
            if (user == null)
                return null;

            //Stored password is hashed via Microsoft.AspNet.Identity so we use their function that compare passwords
            var isPasswordMatch = passwordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Success;
            if (!isPasswordMatch)
                return null;

            return user;
        }
    }

    public class UserManager : IUserManager
    {
        private IUnitOfWork unitOfWork;
        private IPasswordHasher passwordHasher;

        public UserManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
        }

        public Task<AccountManageResult> RegisterAsync(User user)
        {
            return Task.Run(() =>
            {
                try
                {
                    //Check if user with this login exists
                    if (unitOfWork.UserRepository.Get(e => e.Login == user.Login) != null)
                        return new AccountManageResult { IsSuccess = false, Error = "User already exists with this login" };

                    //Check if user with this email address exists
                    if (unitOfWork.UserRepository.Get(e => e.Email == user.Email) != null)
                        return new AccountManageResult { IsSuccess = false, Error = "User already exists with this email address" };

                    //Hash password using Microsoft.AspNet.Identity function 
                    user.Password = passwordHasher.HashPassword(user.Password);
                    unitOfWork.UserRepository.Add(user);
                    return new AccountManageResult { IsSuccess = true };
                }
                catch (Exception e)
                {
                    return new AccountManageResult { IsSuccess = false, Error = e.Message };
                }
            });
        }

        public AccountManageResult ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = unitOfWork.UserRepository.Get(userId);
                if (user == null)
                    return new AccountManageResult { IsSuccess = false, Error = "Problem with user credential." };

                var result = passwordHasher.VerifyHashedPassword(user.Password, oldPassword);
                if (result == PasswordVerificationResult.Failed)
                    return new AccountManageResult { IsSuccess = false, Error = "Wrong old password." };

                user.Password = passwordHasher.HashPassword(newPassword);
                return new AccountManageResult { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new AccountManageResult { IsSuccess = false, Error = e.Message };
            }
        }

        public User GetUserByLogin(string login)
        {
            return unitOfWork.UserRepository.Get(e => e.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return unitOfWork.UserRepository.Get(e => e.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();
        }

        public void Dispose()
        {
            if (unitOfWork != null)
            {
                unitOfWork.Dispose();
                unitOfWork = null;
            }
        }
    }

    public class AlertManager : IAlertManager
    {
        private IUnitOfWork unitOfWork;

        public AlertManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Add(FormAlertViewModel model)
        {
            User user = SessionParser.User;
            Alert alert = new Alert
            {
                AlertType = model.AlertType,
                CreatorLogin = user.Login,
                DateRow = DateTime.Now,
                Message = model.Message,
                Title = model.Title
            };

            unitOfWork.AlertRepository.Add(alert);
        }
    }

    public class RoleManager: IRoleManager
    {

    }

    public class AccountManageResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

    }
}