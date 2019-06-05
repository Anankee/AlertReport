using AlertReport.Db.Interfaces;
using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAlertReportRepository<User> UserRepository { get; set; }
        public IAlertReportRepository<Role> RoleRepository { get; set; }
        public IAlertReportRepository<Alert> AlertRepository { get; set; }
        public IAlertReportRepository<Comment> CommentRepository { get; set; }

        private DbContext DbContext;

        public UnitOfWork(IAlertReportRepository<User> userRepository, IAlertReportRepository<Role> roleRepository,
            IAlertReportRepository<Alert> alertRepository, IAlertReportRepository<Comment> commentRepository)
        {
            DbContext = new AlertReportContext();
            UserRepository = userRepository;
            UserRepository.SetContextAndDbSet(DbContext);
            RoleRepository = roleRepository;
            RoleRepository.SetContextAndDbSet(DbContext);
            AlertRepository = alertRepository;
            AlertRepository.SetContextAndDbSet(DbContext);
            CommentRepository = commentRepository;
            CommentRepository.SetContextAndDbSet(DbContext);
        }

        public void Dispose()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
                DbContext = null;
            }
        }
    }
}
