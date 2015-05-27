using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class CategoriesController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Tree()
        {
            var tree = await CategoryService.FamilyTreeAsync();
            if (tree != null)
                return PartialView(tree);

            ViewData.Model = new ErrorViewModel
            {
                Topic = "خطا",
                Message = CategoryService.Result.Errors.FirstOrDefault(),
            };
            return PartialView("_HandledError");
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Add(CategoryAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            await CategoryService.AddAsync(model);
            if (CategoryService.Result.Succeeded)
            {
                return Json(new { result = true, message = "گروه با موفقیت ایجاد شد." },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = CategoryService.Result.Errors.FirstOrDefault() },
                    JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var cat = await CategoryService.FindByIdAsync(id);
            if (cat != null)
                return PartialView(cat);

            ViewData.Model = new ErrorViewModel
            {
                Topic = "خطا",
                Message = CategoryService.Result.Errors.FirstOrDefault(),
            };
            return PartialView("_HandledError");
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Edit(CategoryEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            await CategoryService.UpdateAsync(model);
            if (CategoryService.Result.Succeeded)
            {
                return Json(new { result = true, message = "گروه با موفقیت ویرایش شد.", id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CategoryService.Result.Errors.FirstOrDefault(), id = model.Id },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<JsonResult> Avatar(CategoryAvatarModel model)
        {
            if (!ModelState.IsValid || !model.FileId.HasValue)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            await CategoryService.ChangeAvatarAsync(model.Id, model.FileId.Value);
            if (CategoryService.Result.Succeeded)
            {
                return Json(new { result = true, message = "آواتار گروه با موفقیت ویرایش شد.", id = model.Id },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CategoryService.Result.Errors.FirstOrDefault(), id = model.Id },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SimpleTree(string targetKeyName)
        {
            ViewBag.TreeTargetKeyName = targetKeyName;
            var tree = await CategoryService.SimpleFamilyTreeAsync();
            if (tree != null)
                return PartialView(tree);

            ViewData.Model = new ErrorViewModel
            {
                Topic = "خطا",
                Message = CategoryService.Result.Errors.FirstOrDefault(),
            };
            return PartialView("_HandledError");
        }

        public async Task<ActionResult> SimpleTreeByKey(string key)
        {
            ViewBag.TargetKey = key;
            var tree = await CategoryService.SimpleFamilyTreeAsync();
            if (tree != null)
                return PartialView(tree);

            ViewData.Model = new ErrorViewModel
            {
                Topic = "خطا",
                Message = CategoryService.Result.Errors.FirstOrDefault(),
            };
            return PartialView("_HandledError");
        }



        // Json

        public async Task<JsonResult> NameJson(string id)
        {
            var searchModel = new CategorySearchModel { Name = id };
            var orderingModel = new OrderingModel<Category>
            {
                OrderByKeySelector = x => x.Name,
                Skip = 0,
                Take = 5,
            };
            var items =
                (await CategoryService.SearchAsync(searchModel, orderingModel)) ?? new List<Category>();
            return Json(items.Select(x => new { x.Id, x.Name }), JsonRequestBehavior.AllowGet);
        }

    }
}