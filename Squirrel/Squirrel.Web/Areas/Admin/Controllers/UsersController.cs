using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(UserSearchModel model, int searchPage = 1)
        {
            var orderingModel = new OrderingModel<User>
            {
                IsAscending = true,
                OrderByKeySelector = x => x.Username,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            var usersTask = UserService.SearchAsync(model, orderingModel);
            var countTask = UserService2.CountAsync(model);

            var users = await usersTask;
            if (users == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = UserService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "searchingUser(#)"
                };
            }
            return PartialView("list", users);
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                 JsonRequestBehavior.AllowGet);
            }

            await UserService.CreateAsync(model.Username, model.Email, model.Password);
            if (UserService.Result.Succeeded)
            {
                return Json(new { result = true, message = "کاربر با موفقیت افزوده شد." },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = UserService.Result.Errors.FirstOrDefault() },
                 JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var user = await UserService.FindByIdAsync(id);
            if (user != null)
                return PartialView(user);


            ViewData.Model = new ErrorViewModel
            {
                Topic = "خطا",
                Message = UserService.Result.Errors.FirstOrDefault(),
            };
            return PartialView("_HandledError");
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Edit(UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            await UserService.UpdateAsync(model);
            if (UserService.Result.Succeeded)
            {
                return Json(new { result = true, message = "کاربر با موفقیت ویرایش شد.", id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true, message = UserService.Result.Errors.FirstOrDefault(), id = model.Id },
                 JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> ResetPassword(UserResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            await UserService.ResetPasswordAsync(model.Id, model.Password);
            if (UserService.Result.Succeeded)
            {
                return Json(new { result = true, message = "پسورد کاربر با موفقیت تغییر کرد.", id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true, message = UserService.Result.Errors.FirstOrDefault(), id = model.Id },
                 JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(UserRemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                 JsonRequestBehavior.AllowGet);
            }

            await UserService.RemoveAsync(model.Id);
            if (UserService.Result.Succeeded)
            {
                return Json(new { result = true, message = "پسورد کاربر با موفقیت حذف شد." },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true, message = UserService.Result.Errors.FirstOrDefault() },
                 JsonRequestBehavior.AllowGet);
        }

    }
}