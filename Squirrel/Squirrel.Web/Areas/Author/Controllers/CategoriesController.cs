using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
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
                ViewBag.SuccessMessage = "دسته با موفقیت افزوده شد.";
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

    }
}