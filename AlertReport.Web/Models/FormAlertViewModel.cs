using AlertReport.Core.Extensions;
using AlertReport.Db.Enums;
using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Models
{
    public class FormAlertViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Display(Name = "Priority")]
        public AlertType AlertType { get; set; }

        public IEnumerable<SelectListItem> GetAlertTypeSelectList(AlertType alertType = AlertType.MINOR)
        {
            foreach (AlertType item in typeof(AlertType).GetEnumValues())
                if (item != AlertType.LOW && item != AlertType.COMPLETED)
                {
                    if (item == alertType)
                        yield return new SelectListItem() { Selected = true, Text = item.GetDisplayName(), Value = item.ToString() };
                    else
                        yield return new SelectListItem() { Text = item.GetDisplayName(), Value = item.ToString() };
                }
        }
    }
}