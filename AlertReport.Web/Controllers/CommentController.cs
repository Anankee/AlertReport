using AlertReport.Db.Models;
using AlertReport.Web.Infrastructure;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Controllers
{
    public class CommentController : BaseController
    {
        private ICommentManager commentManager;

        public CommentController(ICommentManager commentManager)
        {
            this.commentManager = commentManager;
        }

        [HttpGet]
        public PartialViewResult Comment()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Add(FormCommentModel model)
        {
            commentManager.Add(model);

            switch (model.ParentType)
            {
                case CommentParentType.ALERT:
                    return RedirectToAction("AlertComments", new { alertId = model.ParentId });
                case CommentParentType.COMMENT:
                    return RedirectToAction("CommentComments", new { commentId = model.ParentId });                    
                default:
                    return Json(false);
            }
        }

        [HttpGet]
        public ActionResult AlertComments(int alertId)
        {
            IEnumerable<Comment> comments = commentManager.GetByAlert(alertId);
            return PartialView("List", new CommentViewModel
            {
                Comments = comments,
                ParentId = alertId,
                ParentType = CommentParentType.ALERT
            });
        }

        [HttpGet]
        public ActionResult CommentComments(int commentId)
        {
            IEnumerable<Comment> comments = commentManager.GetByComment(commentId);
            return PartialView("SubComment", comments);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (commentManager != null)
            {
                commentManager.Dispose();
                commentManager = null;
            }            
        }
    }
}