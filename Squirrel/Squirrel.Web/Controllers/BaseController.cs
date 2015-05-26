using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Service;
using Squirrel.Utility.Async;
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

        private IFileService _fileService2;
        protected IFileService FileService2
        {
            get { return _fileService2 ?? (_fileService2 = ServiceIOC.Get<IFileService>()); }
        }

        //
        private ITopicService _topicService;
        protected ITopicService TopicService
        {
            get { return _topicService ?? (_topicService = ServiceIOC.Get<ITopicService>()); }
        }

        private ITopicService _topicService2;
        protected ITopicService TopicService2
        {
            get { return _topicService2 ?? (_topicService2 = ServiceIOC.Get<ITopicService>()); }
        }

        //
        private IPostService _postService;
        protected IPostService PostService
        {
            get { return _postService ?? (_postService = ServiceIOC.Get<IPostService>()); }
        }

        private IPostService _postService2;
        protected IPostService PostService2
        {
            get { return _postService2 ?? (_postService2 = ServiceIOC.Get<IPostService>()); }
        }

        //
        private ILogService _logService;
        protected ILogService LogService
        {
            get { return _logService ?? (_logService = ServiceIOC.Get<ILogService>()); }
        }

        private ILogService _logService2;
        protected ILogService LogService2
        {
            get { return _logService2 ?? (_logService2 = ServiceIOC.Get<ILogService>()); }
        }

        //
        private ICommentService _commentService;
        protected ICommentService CommentService
        {
            get { return _commentService ?? (_commentService = ServiceIOC.Get<ICommentService>()); }
        }

        private ICommentService _commentService2;
        protected ICommentService CommentService2
        {
            get { return _commentService2 ?? (_commentService2 = ServiceIOC.Get<ICommentService>()); }
        }

        //
        private ITagService _tagService;
        protected ITagService TagService
        {
            get { return _tagService ?? (_tagService = ServiceIOC.Get<ITagService>()); }
        }

        private ITagService _tagService2;
        protected ITagService TagService2
        {
            get { return _tagService2 ?? (_tagService2 = ServiceIOC.Get<ITagService>()); }
        }

        // Methods
        protected async Task<string> EncryptAsync(string code)
        {
            return
                await Utility.Cryptography.Symmetric<System.Security.Cryptography.TripleDESCryptoServiceProvider>
                    .EncryptAsync(code, "squirrel:basecontroller", "snjab");

        }

        protected async Task<string> DecryptAsync(string token)
        {
            return
                await Utility.Cryptography.Symmetric<System.Security.Cryptography.TripleDESCryptoServiceProvider>
                    .DecryptAsync(token, "squirrel:basecontroller", "snjab");

        }


    }
}