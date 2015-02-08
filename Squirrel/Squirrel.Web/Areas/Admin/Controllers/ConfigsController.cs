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
    public class ConfigsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(string keyword, int searchPage = 1)
        {
            var orderingModel = new OrderingModel<Config>
            {
                IsAscending = true,
                OrderByKeySelector = x => x.Key,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            var configTask = ConfigService.SearchAsync(keyword, orderingModel);
            var countTask = ConfigService2.CountAsync(keyword);

            var config = await configTask;
            if (config == null)
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
            return PartialView("List", config);
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ConfigAddModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await ConfigService.AddAsync(model, User.Identity.Name);
            if (ConfigService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "تنظیم مورد نظر با موفقیت افزوده شد.";
                ViewBag.JsMethod = "ReloadList()";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = ConfigService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var config = await ConfigService.FindByIdAsync(id);
            if (config != null)
                return PartialView(config);

            ViewBag.ErrorMessage = ConfigService.Result.Errors.FirstOrDefault();
            return PartialView("_Message");
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var config = await ConfigService.FindByIdAsync(id);
            if (config == null)
            {
                ViewBag.ErrorMessage = ConfigService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new ConfigEditModel
            {
                Description = config.Description,
                Id = config.Id,
                Key = config.Key,
                Value = config.Value,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ConfigEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await ConfigService.EditAsync(model, User.Identity.Name);
            if (ConfigService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "تنظیم مورد نظر با موفقیت ویرایش شد.";
                ViewBag.JsMethod = string.Format("EditCompleteReload('{0}');", model.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = ConfigService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Remove(Guid id)
        {
            var user = await ConfigService.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = ConfigService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new ConfigRemoveModel
            {
                Id = user.Id
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(ConfigRemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await ConfigService.DeleteAsync(model.Id, User.Identity.Name);
            if (ConfigService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "تنظیم مورد نظر با موفقیت حذف شد.";
                ViewBag.JsMethod = "RemoveCompleteReload();";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = ConfigService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }
    }
}