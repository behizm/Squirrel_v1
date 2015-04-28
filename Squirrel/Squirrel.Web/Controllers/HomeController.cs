using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ExtensionMethods;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Filters;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id)
        {
            var imagedTopics = new List<Topic>();
            foreach (var topic in CachedAppData.LastPublishedTopics.Items.Where(topic => topic.ImageAddress().IsNotNothing()))
            {
                if (imagedTopics.All(x => x.CategoryId != topic.CategoryId))
                {
                    imagedTopics.Add(topic);
                }
                if (imagedTopics.Count >= 3)
                {
                    break;
                }
            }
            ViewBag.TopTopics = imagedTopics;
            return View();
        }

        public ActionResult LastTopics(int page)
        {
            ViewBag.CurrentPage = page;
            ViewData.Model = CachedAppData.LastPublishedTopics.Items.Skip((page - 1) * 5).Take(5).ToList();
            return PartialView("Index_TopicItems");
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [UpdateCachedDataFilter(UpdateCachedDataType.OnActionExecuting)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}