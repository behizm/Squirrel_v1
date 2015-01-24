using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login(string r)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginModel model)
        {
            var login = await UserService.LoginAsync(model.Username, null, model.Password);
            if (login != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    model.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false,
                    login);
                var encTicket = FormsAuthentication.Encrypt(authTicket);
                var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);
            }
            return RedirectToAction("index", "home");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }
    }
}