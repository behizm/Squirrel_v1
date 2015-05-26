using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.FarsiTools;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class TopicsController : BaseController
    {
        public async Task<ActionResult> Search(TopicSearchModel model, int page = 1)
        {
            var ordering = new OrderingModel<Topic, DateTime>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate,
                Skip = (page - 1) * 10,
                Take = 10,
            };

            if (!User.Identity.IsAdmin)
            {
                model.Username = User.Identity.Name;
            }

            var itemsTask = TopicService.SearchAsync(model, ordering);
            var countTask = TopicService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewBag.ErrorMessage = TopicService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = page,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "loadTopics(#)"
                };
            }
            return PartialView("List", items);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(TopicSearchModel model, string sortingBy, int searchPage)
        {
            OrderingModel<Topic, DateTime> ordering1;
            OrderingModel<Topic, DateTime?> ordering2;
            OrderingModel<Topic, string> ordering3;
            const int pageSize = 10;

            switch (sortingBy)
            {
                case "1":
                    ordering1 = new OrderingModel<Topic, DateTime>
                    {
                        IsAscending = false,
                        OrderByKeySelector = x => x.CreateDate,
                        Skip = (searchPage - 1) * pageSize,
                        Take = pageSize,
                    };
                    ordering2 = null;
                    ordering3 = null;
                    break;

                case "2":
                    ordering1 = new OrderingModel<Topic, DateTime>
                    {
                        IsAscending = true,
                        OrderByKeySelector = x => x.CreateDate,
                        Skip = (searchPage - 1) * pageSize,
                        Take = pageSize,
                    };
                    ordering2 = null;
                    ordering3 = null;
                    break;

                case "3":
                    ordering2 = new OrderingModel<Topic, DateTime?>
                    {
                        IsAscending = false,
                        OrderByKeySelector = x => x.PublishDate,
                        Skip = (searchPage - 1) * pageSize,
                        Take = pageSize,
                    };
                    ordering1 = null;
                    ordering3 = null;
                    break;

                case "4":
                    ordering2 = new OrderingModel<Topic, DateTime?>
                    {
                        IsAscending = true,
                        OrderByKeySelector = x => x.PublishDate,
                        Skip = (searchPage - 1) * pageSize,
                        Take = pageSize,
                    };
                    ordering1 = null;
                    ordering3 = null;
                    break;

                case "5":
                    ordering3 = new OrderingModel<Topic, string>
                    {
                        IsAscending = false,
                        OrderByKeySelector = x => x.Title,
                        Skip = (searchPage - 1) * pageSize,
                        Take = pageSize,
                    };
                    ordering2 = null;
                    ordering1 = null;
                    break;

                case "6":
                    ordering3 = new OrderingModel<Topic, string>
                    {
                        IsAscending = true,
                        OrderByKeySelector = x => x.Title,
                        Skip = (searchPage - 1) * pageSize,
                        Take = pageSize,
                    };
                    ordering2 = null;
                    ordering1 = null;
                    break;

                default:
                    ViewData.Model = new ErrorViewModel
                    {
                        Topic = "خطا",
                        Message = ServiceMessages.General_LackOfInputData,
                    };
                    return PartialView("_HandledError");
            }

            if (!User.Identity.IsAdmin)
            {
                model.Username = User.Identity.Name;
            }

            Task<List<Topic>> itemsTask;
            if (ordering1 != null)
            {
                itemsTask = TopicService.SearchAsync(model, ordering1);
            }
            else if (ordering2 != null)
            {
                itemsTask = TopicService.SearchAsync(model, ordering2);
            }
            else
            {
                itemsTask = TopicService.SearchAsync(model, ordering3);
            }
            var countTask = TopicService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطای دریافت اطلاعات",
                    Message = ServiceMessages.General_LackOfInputData,
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
                    PagingMethod = "searchInTopics(#)"
                };
            }
            return PartialView("List", items);
        }

        public async Task<ActionResult> SimpleSearch(TopicSearchModel model, int page = 1)
        {
            var ordering = new OrderingModel<Topic, DateTime>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate,
                Skip = (page - 1) * 10,
                Take = 10,
            };

            if (!User.Identity.IsAdmin)
            {
                model.Username = User.Identity.Name;
            }

            var itemsTask = TopicService.SearchAsync(model, ordering);
            var countTask = TopicService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewBag.ErrorMessage = TopicService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = page,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "loadTopics(#)"
                };
            }
            return PartialView("SimpleList ", items);
        }

        [UpdateCachedDataFilter]
        public async Task<JsonResult> Publish(Guid id)
        {
            await TopicService.PublishAsync(id, User.Identity.Name);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true, message = "عنوان مورد نظر منتشر شد." },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        [UpdateCachedDataFilter]
        public async Task<JsonResult> UnPublish(Guid id)
        {
            await TopicService.UnPublishAsync(id, User.Identity.Name);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true, message = "انتشار عنوان مورد نظر لغو شد." },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(TopicAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                 JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            await TopicService.AddAsync(model);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true, message = "عنوان با موفقیت افزوده شد." },
                JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PublishDate(Guid id)
        {
            var item = await TopicService.FindByIdAsync(id);
            if (item == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            var model = new TopicEditModel
            {
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name,
                PostsOrdering = item.PostsOrdering,
                Title = item.Title,
                Id = item.Id,
            };
            if (item.PublishDate.HasValue)
            {
                model.PublishPersianDate =
                    ((PersianDate)item.PublishDate.Value).ToStringDateTime(timeFormat: PersianTimeFormat.HH_MM_SS);
            }
            return PartialView(model);
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var item = await TopicService.FindByIdAsync(id);
            if (item == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            var model = new TopicEditModel
            {
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name,
                PostsOrdering = item.PostsOrdering,
                Title = item.Title,
                Id = item.Id,
            };
            if (item.PublishDate.HasValue)
            {
                model.PublishPersianDate =
                    ((PersianDate)item.PublishDate.Value).ToStringDateTime(timeFormat: PersianTimeFormat.HH_MM_SS);
            }
            return PartialView(model);
        }

        [HttpPost, ValidateAntiForgeryToken, UpdateCachedDataFilter]
        public async Task<ActionResult> Edit(TopicEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                 JsonRequestBehavior.AllowGet);
            }

            if (model.PublishPersianDate.IsNotNothing())
            {
                var date = model.PublishPersianDate.Split(' ')[0];
                var time = model.PublishPersianDate.Split(' ')[1];
                model.PublishDateTime = new PersianDate(date, time);
            }

            model.Username = User.Identity.Name;
            await TopicService.EditAsync(model);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true, message = "عنوان با موفقیت ویرایش شد.", itemId = model.Id.ToString() },
                JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ListItem(Guid id)
        {
            var item = await TopicService.FindByIdAsync(id);
            if (item == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }
            ViewData.Model = item;
            return PartialView();
        }

        public async Task<ActionResult> Posts(Guid id)
        {
            var items = await TopicService.Posts(id);
            if (items == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            ViewBag.TopicId = id;
            ViewData.Model = items;
            return PartialView();
        }

        public async Task<JsonResult> AddPost(Guid id)
        {
            var post = new PostAddSimpleModel
            {
                Body = ".",
                Username = User.Identity.Name,
                TopicId = id,
            };
            await PostService.AddAsync(post);
            if (PostService.Result.Succeeded)
            {
                return Json(new { result = true, message = "پست با موفقیت افزوده شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = PostService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var item = await TopicService.FindByIdAsync(id);
            if (item == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }
            ViewData.Model = item;
            return PartialView();
        }

        [UpdateCachedDataFilter]
        public async Task<JsonResult> Remove(Guid id)
        {
            await TopicService.DeleteAsync(new TopicDeleteModel { Id = id }, User.Identity.UserId);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true, message = "عنوان با موفقیت حذف شد." },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);

        }

    }
}