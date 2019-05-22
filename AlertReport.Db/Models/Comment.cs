using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string CreatorLogin { get; set; }
        public string CreatorName { get; set; }
        public DateTime RowDate { get; set; }
        public Alert Alert { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ViewsHistory { get; set; }

        public string GetAddedTime()
        {
            var dateDiff = new DateTime((DateTime.Now - RowDate).Ticks);

            if (dateDiff.Year > 1)
                return string.Format("{0}", RowDate.ToString("dd MMMM yyyy"));
            else if (dateDiff.Day == 2)
                return "Wczoraj";
            else if (dateDiff.Day <= 8 || dateDiff.Day != 1)
                return RowDate.ToString("dddd");
            else if (dateDiff.Day > 8)
                return RowDate.ToString("dd MMMM");
            else
                return RowDate.ToString("H:mm");
        }
    }
}
