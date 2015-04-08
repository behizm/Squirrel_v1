using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BotDetect.Web.UI;
using BotDetect.Web.UI.Mvc;
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

        [CaptchaValidation("Captcha", "SampleCaptcha", "Incorrect CAPTCHA code!")]
        public async Task<JsonResult> AddComment(CommentAddPublicModel model)
        {
            if (ModelState["Captcha"].Errors.Any())
            {
                return Json(new { result = false, message = "عبارت امنیتی را به درستی وارد کنید." },
                    JsonRequestBehavior.AllowGet);
            }

            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            var item = new CommentAddModel
            {
                Body = model.Body,
                Email = model.Email,
                IsConfirmed = false,
                IsRead = false,
                Name = model.Name,
                PostId = model.PostId,
                Username = User.Identity.IsAuthenticated ? User.Identity.Name : null,
            };

            await CommentService.AddAsync(item);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault() },
                    JsonRequestBehavior.AllowGet);
        }
    }
}