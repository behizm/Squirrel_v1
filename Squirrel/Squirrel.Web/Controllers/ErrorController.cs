using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult AccessDeniedPartial()
        {
            return PartialView("AccessDenied");
        }
    }
}