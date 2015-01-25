using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

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
                ViewBag.SuccessMessage = "با موفقیت افزوده شد.";
                ViewBag.JsMethod = "ReloadList(1)";
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
                KeySelector = x => x.Username,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };
            var users = await UserService.SearchAsync(model, orderingModel);
            if (users == null)
            {
                ViewBag.ErrorMessage = UserService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await UserService.CountAsync(model);
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
    }
}