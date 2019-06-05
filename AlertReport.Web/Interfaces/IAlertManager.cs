using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlertReport.Db.Enums;
using AlertReport.Db.Models;
using AlertReport.Web.Models;

namespace AlertReport.Web.Interfaces
{
    public interface IAlertManager
    {
        void Add(FormAlertViewModel model);
        IEnumerable<Alert> Get();
        IEnumerable<Alert> Get(AlertType alertType);
        void Dispose();
        void UpdateType(int id, AlertType alertType);
    }
}