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
        public const int IssuesPageItemCount = 3;

        public ActionResult Archive()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Archive(TopicPublishedSearchModel model, int searchPage)
        {
            var ordering = new OrderingModel<Topic, DateTime?>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.PublishDate,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            var topics = await TopicService.SearchInPublishedAsync(model, ordering);
            if (topics == null)
            {
                ViewBag.ErrorMessage = TopicService.Result.Errors.FirstOrDefault();
                topics = new List<Topic>();
            }
            ViewData.Model = topics;
            return PartialView("Archive_Result");
        }

        public async Task<ActionResult> Category(string id, int p = 1)
        {
            var topics = await CategoryService.PublishedTopicsAsync(id, true, (p - 1) * 10, 10);
            if (topics == null)
            {
                ViewBag.ErrorMessage = CategoryService.Result.Errors.FirstOrDefault();
                topics = new List<Topic>();
            }
            ViewData.Model = topics;
            return View();
        }

        public async Task<ActionResult> Tag(string id, int p = 1)
        {
            var topics = await TagService.PublishedTopicsAsync(id, (p - 1) * 10, 10);
            if (topics == null)
            {
                ViewBag.ErrorMessage = TagService.Result.Errors.FirstOrDefault();
                topics = new List<Topic>();
            }
            ViewData.Model = topics;
            return View();
        }

        public ActionResult Date()
        {
            return View();
        }

        public ActionResult User(string id, int p = 1)
        {
            ViewBag.IssuesUser = id;
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