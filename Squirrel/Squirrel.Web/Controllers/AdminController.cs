using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Panel", new { area = "Author" });
        }
    }
}