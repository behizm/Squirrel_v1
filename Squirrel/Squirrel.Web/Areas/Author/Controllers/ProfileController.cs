using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class ProfileController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            var profile = User.Identity.Profile ?? new Profile();
            return PartialView(profile);
        }

        public ActionResult Edit()
        {
            var profile = User.Identity.Profile;
            if (profile == null)
            {
                return PartialView("Create");
            }

            ViewData.Model = new ProfileEditModel
            {
                Firstname = profile.Firstname,
                Lastname = profile.Lastname,
                Id = profile.UserId,
            };
            return PartialView();
        }
    }
}