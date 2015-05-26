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
using Squirrel.Web.Controllers;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class CommentsController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var statsTask = CommentService.Statistics(new CommentStatisticsModel { AuthorId = User.Identity.UserId });
            var unreadedTask = CommentService2.Topics(new CommentSearchModel
            {
                AuthorId = User.Identity.UserId,
                IsRead = false,
            });

            var statistics = await statsTask;
            if (statistics == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا در اطلاعات",
                    Message = CommentService.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            var unreaded = await unreadedTask;
            if (unreaded == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا در اطلاعات",
                    Message = CommentService2.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            ViewBag.CommentsStatistics = statistics;
            ViewData.Model = unreaded;
            return View();
        }

        public async Task<ActionResult> Post(Guid id)
        {
            var post = await PostService.FindByIdAsync(id);
            if (post == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا در اطلاعات",
                    Message = PostService.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }
            ViewData.Model = post;
            return View();
        }

        public async Task<ActionResult> Item(Guid id)
        {
            var itemTask = CommentService.FindByIdAsync(id);
            var answerTask =
                CommentService2.SearchAsync(
                    new CommentSearchModel
                    {
                        UserId = User.Identity.UserId,
                        ParentId = id
                    },
                    new OrderingModel<Comment, DateTime>
                    {
                        OrderByKeySelector = x => x.CreateDate,
                        Skip = 0,
                        Take = 1,
                    });

            var item = await itemTask;
            if (item == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطای دریافت اطلاعات",
                    Message = CommentService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            var answerList = await answerTask;
            if (answerList == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطای دریافت اطلاعات",
                    Message = CommentService2.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            if (answerList.Any())
            {
                ViewBag.AdminAnswer = answerList.First();
            }
            ViewData.Model = item;
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(CommentSearchModel model, int searchPage = 1)
        {
            const int pageSize = 10;

            if (!model.PostId.HasValue)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = ServiceMessages.General_LackOfInputData,
                };
                return PartialView("_HandledError");
            }

            var orderingModel = new OrderingModel<Comment, DateTime>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate,
                Skip = (searchPage - 1) * pageSize,
                Take = pageSize,
            };

            model.AuthorName = User.Identity.IsAdmin ? string.Empty : User.Identity.Name;
            var itemsTask = CommentService.SearchAsync(model, orderingModel);
            var countTask = CommentService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطای دریافت اطلاعات",
                    Message = CommentService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % pageSize == 0 ? count.Value / pageSize : (count.Value / pageSize) + 1,
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

            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault(), id = model.ParentId },
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

            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault(), id = model.Id },
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

            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault(), id = model.Id },
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
            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault() },
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
                return
                    Json(
                        new
                        {
                            result = true,
                            message = "نظر با موفقیت تائید شد.",
                            date = DateTime.Now.ToPersianDate().ToStringDateTime()
                        }, JsonRequestBehavior.AllowGet);
            }
            return
                Json(
                    new
                    {
                        result = false,
                        message = CommentService.Result.Errors.FirstOrDefault(),
                        date = DateTime.Now.ToPersianDate().ToStringDateTime()
                    },
                    JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Unconfirm(Guid id)
        {
            var model = new CommentConfirmModel
            {
                Id = id,
                Username = User.Identity.Name,
                IsConfirm = false,
            };
            await CommentService.ChangeConfirmState(model);
            if (CommentService.Result.Succeeded)
            {
                return
                    Json(
                        new
                        {
                            result = true,
                            message = "عدم تائید نظر با موفقیت  ثبت شد.",
                            date = DateTime.Now.ToPersianDate().ToStringDateTime()
                        }, JsonRequestBehavior.AllowGet);
            }
            return
                Json(
                    new
                    {
                        result = false,
                        message = CommentService.Result.Errors.FirstOrDefault(),
                        date = DateTime.Now.ToPersianDate().ToStringDateTime()
                    },
                    JsonRequestBehavior.AllowGet);
        }
    }
}