using AlertReport.Db.Enums;
using AlertReport.Web.Infrastructure;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Controllers
{
    [AlertAuthorization]
    public class AlertController : Controller
    {
        private IAlertManager alertManager;

        public AlertController(IAlertManager alertManager)
        {
            this.alertManager = alertManager;
        }
        
        [HttpGet]
        public ActionResult Index(AlertType? alertType)
        {
            return View(GetAlertViewModel(alertType));
        }

        [HttpGet]
        public ActionResult List(AlertType? alertType)
        {
            return PartialView(GetAlertViewModel(alertType));
        }

        [HttpPost]
        public void UpdateType(int id, AlertType alertType)
        {
            alertManager.UpdateType(id, alertType);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (alertManager != null)
            {
                alertManager.Dispose();
                alertManager = null;
            }
        }

        #region Helpers
        private AlertViewModel GetAlertViewModel(AlertType? alertType)
        {
            return new AlertViewModel
            {
                Alerts = alertType != null ? alertManager.Get((AlertType)alertType) : alertManager.Get()
            };
        }
        #endregion
    }
}