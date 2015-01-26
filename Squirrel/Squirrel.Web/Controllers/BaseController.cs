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
        
        private IUserService _userService2;
        protected IUserService UserService2
        {
            get { return _userService2 ?? (_userService2 = ServiceIOC.Get<IUserService>()); }
        }

        private IProfileService _profileService;
        protected IProfileService ProfileService
        {
            get { return _profileService ?? (_profileService = ServiceIOC.Get<IProfileService>()); }
        }

    }
}