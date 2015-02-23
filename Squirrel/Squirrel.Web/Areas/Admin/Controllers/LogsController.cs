using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Areas.Admin.Controllers
{
    public class LogsController : Controller
    {
        // GET: Admin/Logs
        public ActionResult Index()
        {
            return View();
        }
    }
}