using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertReport.Db.Models;
using AlertReport.Web.Models;

namespace AlertReport.Web.Interfaces
{
    public interface ICommentManager
    {
        IEnumerable<Comment> GetByAlert(int id);
        IEnumerable<Comment> GetByComment(int commentId);
        void Add(FormCommentModel model);
        void Dispose();
    }
}
