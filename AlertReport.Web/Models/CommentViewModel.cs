using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlertReport.Web.Models
{
    public class CommentViewModel
    {
        public int ParentId { get; set; }
        public CommentParentType ParentType { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }

    public class FormCommentModel
    {
        public int ParentId { get; set; }     
        public string Message { get; set; }
        public CommentParentType ParentType { get; set; }
    }


    public enum CommentParentType
    {
        COMMENT, ALERT
    }
}