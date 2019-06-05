using AlertReport.Core.Extensions;
using AlertReport.Db.Enums;
using AlertReport.Db.Models;
using AlertReport.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Models
{
    public class AlertViewModel
    {
        public IEnumerable<Alert> Alerts { get; set; }
        public AlertType? SelectedAlertType { get; set; }
        public bool IsModerator { get; set; }

        public AlertViewModel()
        {
            IsModerator = SessionParser.User.UserRoles.Any(e => e.Role.Name == "Admin");
        }

        public IEnumerable<SelectListItem> GetAlertTypeSelectList(AlertType alertType)
        {
            foreach (AlertType item in typeof(AlertType).GetEnumValues())
                    yield return new SelectListItem() { Text = item.GetDisplayName(), Value = item.ToString(), Selected = item == alertType };
        }

        public string AlertColor(Alert alert)
        {
            switch (alert.AlertType)
            {
                case AlertType.MINOR:
                    return "bg-warning";
                case AlertType.MAJOR:
                    return "bg-danger";
                case AlertType.LOW:
                    return "bg-info";
                case AlertType.COMPLETED:
                    return "bg-secondary";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}