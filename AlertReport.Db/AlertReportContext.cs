using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db
{
    public class AlertReportContext : DbContext
    {       

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
