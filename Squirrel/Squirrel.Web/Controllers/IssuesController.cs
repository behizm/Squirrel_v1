using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class IssuesController : BaseController
    {
        public const int IssuesPageItemCount = 12;

        public async Task<ActionResult> Category(string id, int p = 1)
        {
            var errorModel = new ErrorViewModel
            {
                Topic = "موردی یافت نشد.",
                Message = string.Format("گروهی با نام « {0} » وجود ندارد.", id),
            };

            var itemsModel =
                await CategoryService.PublishedTopicsAsync(id, true, (p - 1) * IssuesPageItemCount, IssuesPageItemCount);
            if (itemsModel == null)
            {
                return View("HandledError", errorModel);
            }

            ViewBag.CategoryName = id;
            ViewBag.SearchResults = itemsModel.List;
            ViewBag.CurrentPage = p;
            ViewBag.TotalPages = itemsModel.CountOfAll % IssuesPageItemCount == 0
                ? itemsModel.CountOfAll / IssuesPageItemCount
                : (itemsModel.CountOfAll / IssuesPageItemCount) + 1;

            return View();
        }

        public async Task<ActionResult> Tag(string id, int p = 1)
        {
            var errorModel = new ErrorViewModel
            {
                Topic = "موردی یافت نشد.",
                Message = string.Format("برچسبی با نام « {0} » وجود ندارد.", id),
            };

            var items = await TagService.PublishedTopicsAsync(id, (p - 1) * IssuesPageItemCount, IssuesPageItemCount);
            if (items == null)
            {
                return View("HandledError", errorModel);
            }

            ViewBag.TagName = id;
            ViewBag.SearchResults = items.List;
            ViewBag.CurrentPage = p;
            ViewBag.TotalPages = items.CountOfAll % IssuesPageItemCount == 0
                ? items.CountOfAll / IssuesPageItemCount
                : (items.CountOfAll / IssuesPageItemCount) + 1;

            return View();
        }

        public async Task<ActionResult> Author(string id, int p = 1)
        {
            var errorModel = new ErrorViewModel
            {
                Topic = "نویسنده مورد نظر یافت نشد.",
            };
            Guid guid;
            var parseResult = Guid.TryParse(id, out guid);
            if (!parseResult)
            {
                return View("HandledError", errorModel);
            }

            var user = await UserService.FindByIdAsync(guid);
            if (user == null)
            {
                return View("HandledError", errorModel);
            }

            var items = await UserService.PublishedTopicsAsync(user.Id, (p - 1) * IssuesPageItemCount, IssuesPageItemCount);
            if (items == null)
            {
                return View("HandledError", errorModel);
            }

            ViewBag.Author = user;
            ViewBag.SearchResults = items.List;
            ViewBag.CurrentPage = p;
            ViewBag.TotalPages = items.CountOfAll % IssuesPageItemCount == 0
                ? items.CountOfAll / IssuesPageItemCount
                : (items.CountOfAll / IssuesPageItemCount) + 1;

            return View();
        }

        public async Task<ActionResult> Search(string id, int p = 1)
        {
            var searchModel = new TopicSearchModel
            {
                Title = id,
            };
            var orderingModel = new OrderingModel<Topic, DateTime?>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.PublishDate,
                Skip = (p - 1) * IssuesPageItemCount,
                Take = IssuesPageItemCount,
            };
            var viewResult = await SearchView(searchModel, orderingModel, p);
            if (viewResult != null)
            {
                return viewResult;
            }

            ViewBag.SearchKeyword = id;
            return View();
        }

        private async Task<ViewResult> SearchView(TopicSearchModel searchModel, OrderingModel<Topic, DateTime?> orderingModel, int page)
        {
            var errorModel = new ErrorViewModel
            {
                Topic = "در هنگام پردازش صفحه خطایی رخ داد.",
                Message = "لطفا کمی دیرتر دوباره صفحه مورد نظر خود را باز کنید.",
            };

            var itemsTask = TopicService.SearchAsync(searchModel, orderingModel);
            var countTask = TopicService2.CountAsync(searchModel);
            var items = await itemsTask;
            if (items == null)
            {
                return View("HandledError", errorModel);
            }
            var count = await countTask;
            if (count == null)
            {
                return View("HandledError", errorModel);
            }

            ViewBag.SearchResults = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = count % IssuesPageItemCount == 0
                ? count / IssuesPageItemCount
                : (count / IssuesPageItemCount) + 1;

            return null;
        }

    }
}