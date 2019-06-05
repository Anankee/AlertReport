using AlertReport.Db.Enums;
using AlertReport.Db.Interfaces;
using AlertReport.Db.Models;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace AlertReport.Web.Infrastructure
{
    public abstract class BaseManager : IDisposable
    {
        protected IUnitOfWork unitOfWork;

        public BaseManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
    
    public class AccountManager : BaseManager, IAccountManager
    {
        private readonly IPasswordHasher passwordHasher;

        public AccountManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : base(unitOfWork)
        {
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
        
        private User GetUserFromDb(string loginOrEmail, string password)
        {
            //First find user by login or email address
            var user = unitOfWork.UserRepository.Get(e => e.Login == loginOrEmail || e.Email == loginOrEmail)
                .Include(e=>e.UserRoles.Select(s=>s.Role))
                .SingleOrDefault();

            if (user == null)
                return null;

            //Stored password is hashed via Microsoft.AspNet.Identity so we use their function that compare passwords
            var isPasswordMatch = passwordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Success;
            if (!isPasswordMatch)
                return null;

            return user;
        }
    }

    public class UserManager : BaseManager, IUserManager
    {
        private IPasswordHasher passwordHasher;

        public UserManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : base(unitOfWork)
        {
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
            return unitOfWork.UserRepository.Get(e => e.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase))
                .Include(e => e.UserRoles.Select(s => s.Role))
                .SingleOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return unitOfWork.UserRepository.Get(e => e.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                .Include(e => e.UserRoles.Select(s => s.Role))
                .SingleOrDefault();
        }
    }

    public class AlertManager : BaseManager, IAlertManager
    {
        public AlertManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Add(FormAlertViewModel model)
        {
            User dbUser = unitOfWork.UserRepository.Get(SessionParser.User.Id);
            Alert alert = new Alert
            {
                AlertType = model.AlertType,
                User = dbUser,
                DateRow = DateTime.Now,
                Message = model.Message,
                Title = model.Title
            };

            unitOfWork.AlertRepository.Add(alert);
        }

        public IEnumerable<Alert> Get()
        {
            return unitOfWork.AlertRepository.GetAll()
                .Include(e => e.User);
        }

        public IEnumerable<Alert> Get(AlertType alertType)
        {
            return unitOfWork.AlertRepository.Get(e => e.AlertType == alertType)
                .Include(e=>e.User);
        }

        public void UpdateType(int id, AlertType alertType)
        {
            var dbEntity = unitOfWork.AlertRepository.Get(id);
            dbEntity.AlertType = alertType;
            unitOfWork.AlertRepository.Update(dbEntity);
        }
    }

    public class CommentManager : BaseManager, ICommentManager
    {
        public CommentManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void Add(FormCommentModel model)
        {
            Comment comment = new Comment
            {
                User = unitOfWork.UserRepository.Get(SessionParser.User.Id),
                Message = model.Message,
                RowDate = DateTime.Now
            };

            switch (model.ParentType)
            {
                case CommentParentType.ALERT:
                    comment.Alert = unitOfWork.AlertRepository.Get(model.ParentId);
                    break;
                case CommentParentType.COMMENT:
                    comment.ParentComment = unitOfWork.CommentRepository.Get(model.ParentId);
                    break;
            }

            unitOfWork.CommentRepository.Add(comment);
        }

        public IEnumerable<Comment> GetByAlert(int id)
        {
            return unitOfWork.CommentRepository.Get(e => e.Alert.Id == id)
                .Include(e=>e.ParentComment)
                .Include(e=>e.Comments)
                .Include(e=>e.User);
        }

        public IEnumerable<Comment> GetByComment(int id)
        {
            return unitOfWork.CommentRepository.Get(e => e.ParentComment.Id == id)
                .Include(e => e.ParentComment)
                .Include(e => e.Comments)
                .Include(e => e.User);
        }
    }

    public class RoleManager : BaseManager, IRoleManager
    {
        public RoleManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }

    public class AccountManageResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

    }
}