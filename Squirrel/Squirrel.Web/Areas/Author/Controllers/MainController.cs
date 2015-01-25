using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class MainController : Controller
    {
        // GET: Author/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}