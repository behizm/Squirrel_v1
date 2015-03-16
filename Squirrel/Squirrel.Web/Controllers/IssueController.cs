using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Controllers
{
    public class IssueController : BaseController
    {

        public async Task<ActionResult> Index(string id)
        {
            var topic = await TopicService.FindByIssueIdAsync(id);
            if (topic == null || !topic.IsPublished)
            {
                return View("NotFound");
            }

            return View("Item", topic);
        }

        public ActionResult Item(Guid id)
        {
            ViewBag.ThisId = id;
            return View();
        }

        public ActionResult Perview(Guid id)
        {
            return View("Item");
        }
    }
}