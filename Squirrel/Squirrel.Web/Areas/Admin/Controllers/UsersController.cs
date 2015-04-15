using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;

namespace Squirrel.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await UserService.CreateAsync(model.Username, model.Email, model.Password);
            if (UserService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "کاربر با موفقیت افزوده شد.";
                ViewBag.JsMethod = "ReloadList()";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
            return PartialView(model);
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
                ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "LoadList(#)"
                };
            }
            return PartialView("list", users);
        }

        public async Task<bool> Active(Guid id)
        {
            await UserService.ActiveAsync(id);
            if (UserService.Result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> Lock(Guid id)
        {
            await UserService.LockAsync(id);
            if (UserService.Result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> Unlock(Guid id)
        {
            await UserService.UnlockAsync(id);
            if (UserService.Result.Succeeded)
                return true;
            return false;
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var user = await UserService.FindByIdAsync(id);
            if (user != null)
                return PartialView(user);

            ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
            return PartialView("_Message");
        }

        public async Task<bool> AddAdmin(Guid id)
        {
            await UserService.ChangeAdminAsync(id, true);
            if (UserService.Result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> RemoveAdmin(Guid id)
        {
            await UserService.ChangeAdminAsync(id, false);
            if (UserService.Result.Succeeded)
                return true;
            return false;
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var user = await UserService.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new UserUpdateModel
            {
                Email = user.Email,
                Id = user.Id,
                Username = user.Username,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<ActionResult> Edit(UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await UserService.UpdateAsync(model.Id, model.Username, model.Email);
            if (UserService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "کاربر با موفقیت ویرایش شد.";
                ViewBag.JsMethod = string.Format("EditCompleteReload('{0}');", model.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Remove(Guid id)
        {
            var user = await UserService.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new UserRemoveModel
            {
                Id = user.Id
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(UserRemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await UserService.RemoveAsync(model.Id);
            if (UserService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "کاربر با موفقیت حذف شد.";
                ViewBag.JsMethod = "RemoveCompleteReload();";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> ResetPassword(Guid id)
        {
            var user = await UserService.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new UserResetPasswordModel
            {
                Id = user.Id
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(UserResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await UserService.ResetPasswordAsync(model.Id, model.Password);
            if (UserService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "پسورد کاربر با موفقیت تغییر کرد.";
                ViewBag.JsMethod = string.Format("LoadDetails('{0}');", model.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

    }
}