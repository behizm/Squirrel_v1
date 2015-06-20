using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using BotDetect.Web;
using BotDetect.Web.UI.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ExtensionMethods;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Filters;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id)
        {
            var imagedTopics = new List<Topic>();
            foreach (var topic in CachedAppData.LastPublishedTopics.Items.Where(topic => topic.ImageAddress().IsNotNothing()))
            {
                if (imagedTopics.All(x => x.CategoryId != topic.CategoryId))
                {
                    imagedTopics.Add(topic);
                }
                if (imagedTopics.Count >= 3)
                {
                    break;
                }
            }
            ViewBag.TopTopics = imagedTopics;
            return View();
        }

        public ActionResult LastTopics(int page)
        {
            ViewBag.CurrentPage = page;
            ViewData.Model = CachedAppData.LastPublishedTopics.Items.Skip((page - 1) * 5).Take(5).ToList();
            return PartialView("Index_TopicItems");
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [UpdateCachedDataFilter(UpdateCachedDataType.OnActionExecuting)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Contactus()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaText", "CaptchaId", "عبارت امنیتی را اشتباه وارد کردید.")]
        public async Task<JsonResult> Contactus(ContactUsViewModel model)
        {
            MvcCaptcha.ResetCaptcha("CaptchaId");
            if (!ModelState.IsValid)
            {
                var captchaError = "";
                if (ModelState["CaptchaText"].Errors.Any())
                {
                    captchaError = "عبارت امنیتی را اشتباه وارد کردید.";
                }

                return Json(new { result = false, captchaError , message = "اطلاعات ورودی ناقص یا نادرست است." },
                    JsonRequestBehavior.AllowGet);
            }

            await EmailService.SendAsync(new EmailSendModel
            {
                Body = model.Body,
                Subject = "Squirrel : " + model.Subject,
                ToAddress = "behnam.zeighami@gmail.com",
            });

            if (EmailService.Result.Succeeded)
            {
                return Json(new { result = true },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = EmailService.Result.Errors.LastOrDefault() },
                    JsonRequestBehavior.AllowGet);
        }
    }
}