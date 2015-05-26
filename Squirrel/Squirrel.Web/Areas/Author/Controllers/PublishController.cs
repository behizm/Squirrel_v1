using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
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