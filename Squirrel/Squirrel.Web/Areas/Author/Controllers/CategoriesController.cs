using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

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
            var tree = await CategoryService.FamilyTree();
            if (tree != null)
                return PartialView(tree);
            ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
            return PartialView("_Message");
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CategoryAddModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await CategoryService.AddAsync(model);
            if (CategoryService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "گروه با موفقیت افزوده شد.";
                ViewBag.JsMethod = "ReloadTree()";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var cat = await CategoryService.FindByIdAsync(id);
            if (cat != null)
                return PartialView(cat);

            ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
            return PartialView("_Message");
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var category = await CategoryService.FindByIdAsync(id);
            if (category == null)
            {
                ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new CategoryEditModel
            {
                Description = category.Description,
                Id = category.Id,
                Name = category.Name,
                Parent = category.ParentId.HasValue ? category.Parent.Name : null,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            await CategoryService.UpdateAsync(model);
            if (CategoryService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "گروه با موفقیت ویرایش شد.";
                ViewBag.JsMethod = string.Format("EditCompleteReload('{0}');", model.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        // Json

        public async Task<JsonResult> NameJson(string id)
        {
            var searchModel = new CategorySearchModel { Name = id };
            var orderingModel = new OrderingModel<Category>
            {
                KeySelector = x => x.Name,
                Skip = 0,
                Take = 5,
            };
            var items =
                (await CategoryService.SearchAsync(searchModel, orderingModel)) ?? new List<Category>();
            return Json(items.Select(x => new { x.Id, x.Name }), JsonRequestBehavior.AllowGet);
        }

    }
}