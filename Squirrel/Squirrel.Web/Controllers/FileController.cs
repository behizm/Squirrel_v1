using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Controllers
{
    public class FileController : Controller
    {
        
        public ActionResult Req(Guid id)
        {
            return View();
        }

        public ActionResult Req()
        {
            return View();
        }
    }
}