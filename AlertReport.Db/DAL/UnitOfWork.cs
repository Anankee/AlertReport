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

        public UnitOfWork(IAlertReportRepository<User> userRepository, IAlertReportRepository<Role> roleRepository,
            IAlertReportRepository<Alert> alertRepository)
        {
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            AlertRepository = alertRepository;
        }

        public void Dispose()
        {
            if (UserRepository != null)
            {
                UserRepository.Dispose();
                UserRepository = null;
            }                
            if (RoleRepository != null)
            {
                RoleRepository.Dispose();
                RoleRepository = null;
            }
            if (AlertRepository != null)
            {
                AlertRepository.Dispose();
                AlertRepository = null;
            }
        }
    }
}
