using System.Web.Mvc;
using Squirrel.Service;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new ISqPrincipal User
        {
            get { return (ISqPrincipal)base.User; }
        }

        private IUserService _userService;
        protected IUserService UserService
        {
            get { return _userService ?? (_userService = ServiceIOC.Get<IUserService>()); }
        }
    }
}