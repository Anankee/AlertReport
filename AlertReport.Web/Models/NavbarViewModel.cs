using AlertReport.Db.Enums;
using AlertReport.Db.Models;
using AlertReport.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlertReport.Web.Models
{
    public class NavbarViewModel
    {
        public User LoggedUser { get; private set; }
        public bool IsModerator { get; private set; }
        public IEnumerable<Alert> UnreadAlerts { get; set; }
        public IEnumerable<Comment> UnreaderComments { get; set; }
        public IEnumerable<AlertType> AlertTypes { get; private set; }

        public NavbarViewModel()
        {
            UnreadAlerts = new List<Alert>();
            UnreaderComments = new List<Comment>();
            LoggedUser = SessionParser.User;
            AlertTypes = GetAlertTypes();
        }

        private IEnumerable<AlertType> GetAlertTypes()
        {
            foreach (AlertType item in typeof(AlertType).GetEnumValues())
                yield return item;
        }
    }
}