using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAlertReportRepository<User> UserRepository { get; set; }
        IAlertReportRepository<Role> RoleRepository { get; set; }
        IAlertReportRepository<Alert> AlertRepository { get; set; }
        IAlertReportRepository<Comment> CommentRepository { get; set; }
    }
}
