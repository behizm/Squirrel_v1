using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class FilesController : Controller
    {
        // GET: Author/Files
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
    }
}