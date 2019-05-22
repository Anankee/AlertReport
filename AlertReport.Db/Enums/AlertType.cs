using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Enums
{
    public enum AlertType
    {
        [Display(Name = "Minor")]
        MINOR,
        [Display(Name = "Major")]
        MAJOR,
        [Display(Name = "Low")]
        LOW,
        [Display(Name = "Completed")]
        COMPLETED
    }
}
