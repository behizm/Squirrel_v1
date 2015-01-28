using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        public async Task<ActionResult> Remove(Guid id)
        {
            var category = await CategoryService.FindByIdAsync(id);
            if (category == null)
            {
                ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new CategoryRemoveModel
            {
                Id = category.Id
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(CategoryRemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await CategoryService.DeleteAsync(model.Id);
            if (CategoryService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "گروه با موفقیت حذف شد.";
                ViewBag.JsMethod = "RemoveCompleteReload();";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Replace(Guid id)
        {
            var category = await CategoryService.FindByIdAsync(id);
            if (category == null)
            {
                ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new CategoryReplaceModel
            {
                Id = category.Id
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Replace(CategoryReplaceModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            var cat= await CategoryService.ReplaceAsync(model);
            if (cat != null)
            {
                ViewBag.SuccessMessage = "گروه با موفقیت جایگزین شد.";
                ViewBag.JsMethod = string.Format("ReplaceCompleteReload('{0}');", cat.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }
    }
}