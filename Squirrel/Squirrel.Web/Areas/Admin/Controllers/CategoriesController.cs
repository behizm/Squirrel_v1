using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;

namespace Squirrel.Web.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Remove(CategoryRemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            await CategoryService.DeleteAsync(model.Id);
            if (CategoryService.Result.Succeeded)
            {
                return Json(new { result = true, message = "گروه مورد نظر با موفقیت حذف شد.", id = model.Id },
                    JsonRequestBehavior.AllowGet);
            } 
            
            return Json(new { result = false, message = CategoryService.Result.Errors.FirstOrDefault(), id = model.Id },
                 JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Replace(CategoryReplaceModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            var cat= await CategoryService.ReplaceAsync(model);
            if (cat != null)
            {
                return Json(new { result = true, message = "گروه مورد نظر با جایگزین حذف شد.", id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CategoryService.Result.Errors.FirstOrDefault(), id = model.Id },
                JsonRequestBehavior.AllowGet);
        }
    }
}