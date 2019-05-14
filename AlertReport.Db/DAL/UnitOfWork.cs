using AlertReport.Db.Interfaces;
using AlertREport.Db.Models;
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
        
        public UnitOfWork(IAlertReportRepository<User> userRepository)
        {
            UserRepository = userRepository;
        }
    }
}
