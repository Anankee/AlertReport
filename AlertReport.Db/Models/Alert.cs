using AlertReport.Db.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Models
{
    public class Alert
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public AlertType AlertType { get; set; }
        public DateTime DateRow { get; set; }
        public string CreatorLogin { get; set; }
        public string CreatorName { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ViewsHistory { get; set; }

        public string GetAddedTime()
        {
            var dateDiff = new DateTime((DateTime.Now - DateRow).Ticks);

            if (dateDiff.Year > 1)
                return string.Format("{0} o {1}", DateRow.ToString("dd MMMM yyyy"), DateRow.ToString("H:mm"));
            else if (dateDiff.Day == 2)
                return "Wczoraj o " + dateDiff.ToString("H:mm");
            else if (dateDiff.Day > 1)
                return string.Format("{0} o {1}", DateRow.ToString("dd MMMM "), DateRow.ToString("H:mm"));
            else if (dateDiff.Hour > 0)
                return dateDiff.Hour + " godz";
            else if (dateDiff.Minute > 0)
                return dateDiff.Minute + " min";
            else
                return dateDiff.Second + " sek.";
        }
    }
}