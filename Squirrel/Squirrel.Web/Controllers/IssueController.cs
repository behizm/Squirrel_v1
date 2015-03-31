using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using WebGrease.Css.Extensions;

namespace Squirrel.Web.Controllers
{
    public class IssueController : BaseController
    {

        public async Task<ActionResult> Index(string id)
        {
            var topic = await TopicService.FindByIssueIdAsync(id);
            if (topic == null || !topic.IsPublished)
            {
                return View("NotFound");
            }
            topic.Posts = topic.Posts.OrderByPostOrdering(topic.PostsOrdering);
            var privatePosts = topic.Posts.Where(p => !p.IsPublic).ToList();
            privatePosts.ForEach(p => topic.Posts.Remove(p));
            return View("Item", topic);
        }

        public ActionResult Item(Guid id)
        {
            ViewBag.ThisId = id;
            return View();
        }

        public ActionResult Perview(Guid id)
        {
            return View("Item");
        }

        public async Task<JsonResult> AddComment(CommentAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            model.IsConfirmed = false;
            model.IsRead = false;
            model.ParentId = null;
            model.Username = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            await CommentService.AddAsync(model);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault() },
                    JsonRequestBehavior.AllowGet);
        }
    }
}