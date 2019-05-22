using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlertReport.Web.Models;

namespace AlertReport.Web.Interfaces
{
    public interface IAlertManager
    {
        void Add(FormAlertViewModel model);
    }
}