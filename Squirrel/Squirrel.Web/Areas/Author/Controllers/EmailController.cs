using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class EmailController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Send(AdminEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            await EmailService.SendAsync(new EmailSendModel
            {
                Body = model.Body,
                Subject = model.Subject,
                ToAddress = model.Email,
            });

            if (EmailService.Result.Succeeded)
            {
                return Json(new { result = true, message = "ایمیل با موفقیت ارسال شد." },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = EmailService.Result.Errors.LastOrDefault() },
                    JsonRequestBehavior.AllowGet);
        }
    }
}