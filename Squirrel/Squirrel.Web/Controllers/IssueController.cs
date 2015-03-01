using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Controllers
{
    public class IssueController : Controller
    {
        
        public ActionResult Index(Guid id)
        {
            ViewBag.ThisId = id;
            return View();
        }

        public ActionResult Perview(Guid id)
        {
            return View();
        }
    }
}