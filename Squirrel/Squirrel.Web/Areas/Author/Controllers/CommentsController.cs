using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class CommentsController : BaseController
    {
        public async Task<ActionResult> Post(Guid id)
        {
            var post = await PostService.FindByIdAsync(id);
            if (post == null)
            {
                ViewBag.ErrorMessage = PostService.Result.Errors.FirstOrDefault();
                return View();
            }
            ViewData.Model = post;
            return View();
        }

        public async Task<ActionResult> Item(Guid id)
        {
            var item = await CommentService.FindByIdAsync(id);
            if (item == null)
            {
                ViewBag.ErrorMessage = CommentService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }
            ViewData.Model = item;
            return PartialView("SearchItem");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(CommentSearchModel model, int searchPage = 1)
        {
            if (!model.PostId.HasValue)
            {
                ViewBag.ErrorMessage = ServiceMessages.General_LackOfInputData;
                return PartialView("_Message");
            }

            var orderingModel = new OrderingModel<Comment, DateTime>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            model.PostAuthorUsername = User.Identity.Name;
            var itemsTask = CommentService.SearchAsync(model, orderingModel);
            var countTask = CommentService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewBag.ErrorMessage = CommentService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "searchInComment(#)"
                };
            }
            return PartialView("SearchResult", items);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(CommentAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.ParentId },
                 JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            model.IsConfirmed = true;
            model.IsRead = true;
            await CommentService.AddAsync(model);
            if (model.ParentId.HasValue)
            {
                // ReSharper disable once CSharpWarnings::CS4014
                CommentService2.MarkAsRead(new CommentMarkModel { Id = model.ParentId.Value, Username = User.Identity.Name });
            }
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true, message = "نظر با موفقیت افزوده شد.", id = model.ParentId },
                JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault(), id = model.ParentId },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(CommentEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            await CommentService.EditAsync(model);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true, message = "نظر با موفقیت ویرایش شد.", id = model.Id },
                JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault(), id = model.Id },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Remove(CommentRemoveModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            await CommentService.DeleteAsync(model);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true, message = "نظر با موفقیت حذف شد.", id = model.Id },
                JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault(), id = model.Id },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> MarkAsRead(Guid id)
        {
            var model = new CommentMarkModel
            {
                Id = id,
                Username = User.Identity.Name,
            };
            await CommentService.MarkAsRead(model);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true, message = "نظر با موفقیت خوانده شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = PostService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Confirm(Guid id)
        {
            var model = new CommentConfirmModel
            {
                Id = id,
                Username = User.Identity.Name,
                IsConfirm = true,
            };
            await CommentService.ChangeConfirmState(model);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true, message = "نظر با موفقیت تائید شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = PostService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Unconfirm(Guid id)
        {
            var model = new CommentConfirmModel
            {
                Id = id,
                Username = User.Identity.Name,
                IsConfirm = true,
            };
            await CommentService.ChangeConfirmState(model);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true, message = "عدم تائید نظر با موفقیت  ثبت شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = PostService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }
    }
}