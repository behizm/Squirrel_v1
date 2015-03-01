using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;

namespace Squirrel.Web.Controllers
{
    public class IssuesController : BaseController
    {
        public ActionResult Archive()
        {
            return View();
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

        public ActionResult Tag(string id)
        {
            return View();
        }


    }
}