using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Web.Filters;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id)
        {
            return View();
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