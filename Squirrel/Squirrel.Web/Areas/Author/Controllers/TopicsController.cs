using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

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

        public async Task<JsonResult> Publish(Guid id)
        {
            await TopicService.PublishAsync(id, User.Identity.Name);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UnPublish(Guid id)
        {
            await TopicService.UnPublishAsync(id, User.Identity.Name);
            if (TopicService.Result.Succeeded)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = TopicService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(TopicAddModel model)
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

        public async Task<ActionResult> Edit(Guid id)
        {
            var item = await TopicService.FindByIdAsync(id);
            if (item == null)
            {
                ViewBag.ErrorMessage = TopicService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            ViewData.Model = new TopicEditModel
            {
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name,
                PostsOrdering = item.PostsOrdering,
                Title = item.Title,
                Id = item.Id,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TopicEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                 JsonRequestBehavior.AllowGet);
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
                ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }
            ViewData.Model = item;
            return PartialView();
        }

        public async Task<ActionResult> Posts(Guid id)
        {
            var items = await TopicService.Posts(id);
            if (items == null)
            {
                ViewBag.ErrorMessage = TopicService.Result.Errors.FirstOrDefault();
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
                return Json(new { result = true, message = "مطلب با موفقیت افزوده شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = PostService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }

    }
}