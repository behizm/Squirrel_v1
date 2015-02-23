using System;
using System.Web.Mvc;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class PublishController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}