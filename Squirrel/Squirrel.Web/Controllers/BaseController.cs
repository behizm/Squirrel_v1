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

        //
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

        //
        private IConfigService _configService;
        protected IConfigService ConfigService
        {
            get { return _configService ?? (_configService = ServiceIOC.Get<IConfigService>()); }
        }

        private IConfigService _configService2;
        protected IConfigService ConfigService2
        {
            get { return _configService2 ?? (_configService2 = ServiceIOC.Get<IConfigService>()); }
        }

        //
        private ICategoryService _categoryService;
        protected ICategoryService CategoryService
        {
            get { return _categoryService ?? (_categoryService = ServiceIOC.Get<ICategoryService>()); }
        }

        //
        private IProfileService _profileService;
        protected IProfileService ProfileService
        {
            get { return _profileService ?? (_profileService = ServiceIOC.Get<IProfileService>()); }
        }

        //
        private IFileService _fileService;
        protected IFileService FileService
        {
            get { return _fileService ?? (_fileService = ServiceIOC.Get<IFileService>()); }
        }

    }
}