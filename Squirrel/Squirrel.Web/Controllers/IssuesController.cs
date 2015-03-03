using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Web.Controllers
{
    public class IssuesController : BaseController
    {
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


    }
}