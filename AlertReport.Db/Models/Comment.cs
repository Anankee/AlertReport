using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        public DateTime RowDate { get; set; }
        public User User { get; set; }
        public Alert Alert { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ViewsHistory { get; set; }
    }
}
