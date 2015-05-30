using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class ProfileController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            ProfileModelOverrided profile = User.Identity.Profile;
            return PartialView(profile);
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Edit(ProfileEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            await ProfileService.ChangeAsync(model);
            if (ProfileService.Result.Succeeded)
            {
                return Json(new { result = true, message = "اطلاعات با موفقیت ویرایش شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = ProfileService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Avatar(ProfileAvatarModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name.ToLower();
            await ProfileService.ChangeAvatarAsync(model);
            if (ProfileService.Result.Succeeded)
            {
                return Json(new { result = true, message = "عکس پروفایل با موفقیت ویرایش شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = ProfileService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }



    }
}