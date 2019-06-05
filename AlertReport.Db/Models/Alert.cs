using AlertReport.Db.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ViewsHistory { get; set; }
    }
}