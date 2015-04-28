using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.FarsiTools;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Admin.Controllers
{
    public class LogsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(LogSearchModel model, int searchPage)
        {
            var orderingModel = new OrderingModel<Log, DateTime>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            if (model.CreatePersianDateFrom.IsNotNothing())
            {
                var date = model.CreatePersianDateFrom.Split(' ')[0];
                var time = model.CreatePersianDateFrom.Split(' ')[1];
                model.CreateDateFrom = new PersianDate(date, time);
            }

            if (model.CreatePersianDateTo.IsNotNothing())
            {
                var date = model.CreatePersianDateTo.Split(' ')[0];
                var time = model.CreatePersianDateTo.Split(' ')[1];
                model.CreateDateTo = new PersianDate(date, time);
            }

            var itemsTask = LogService.SearchAsync(model, orderingModel);
            var countTask = LogService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewBag.ErrorMessage = LogService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "beginSearch(#)"
                };
            }
            return PartialView("List", items);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var item = await LogService.FindByIdAsync(id);
            if (item != null) return PartialView(item);
            ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
            return PartialView("_Message");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Clean(LogCleanModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            if (model.CleanPersianDateFrom.IsNotNothing())
            {
                var date = model.CleanPersianDateFrom.Split(' ')[0];
                var time = model.CleanPersianDateFrom.Split(' ')[1];
                model.CleanDateFrom = new PersianDate(date, time);
            }

            if (model.CleanPersianDateTo.IsNotNothing())
            {
                var date = model.CleanPersianDateTo.Split(' ')[0];
                var time = model.CleanPersianDateTo.Split(' ')[1];
                model.CleanDateTo = new PersianDate(date, time);
            }

            await LogService.CleanAsync(model);
            if (!LogService.Result.Succeeded)
            {
                return Json(new { result = false, message = LogService.Result.Errors.FirstOrDefault() },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true, message = "لاگ های مورد نظر با موفقیت حذف شد." },
                    JsonRequestBehavior.AllowGet);
        }
    }
}