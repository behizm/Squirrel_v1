using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Service;

namespace Squirrel.Test.ServiceLayer
{
    [TestClass]
    public class PostServiceTest
    {
        private IPostService _postService;
        public IPostService PostService
        {
            get { return _postService ?? (_postService = ServiceIOC.Get<IPostService>()); }
        }

        private IUserService _userService;
        public IUserService UserService
        {
            get { return _userService ?? (_userService = ServiceIOC.Get<IUserService>()); }
        }

        private ITopicService _topicService;
        public ITopicService TopicService
        {
            get { return _topicService ?? (_topicService = ServiceIOC.Get<ITopicService>()); }
        }

        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
            var stringList = AppDomain.CurrentDomain.BaseDirectory.Split('\\').ToList();
            stringList[stringList.Count - 3] = "Squirrel.Web";
            stringList[stringList.Count - 2] = "App_Data";
            stringList.RemoveAt(stringList.Count - 1);
            var path = stringList.Aggregate((i, j) => i + "\\" + j);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }


        [TestMethod]
        public void Create()
        {
            var user = UserService.FindByUsernameAsync("behi8303").Result;
            Assert.IsNotNull(user, UserService.Result.Errors.FirstOrDefault());

            var post = new PostAddModel
            {
                Body = "This is test of post content for game Topic.",
                TopicId = Guid.Parse("ab6f4352-5104-4149-89da-144f7bb9f433"),
                UserId = user.Id,
                Tags = new List<string> { "game", " pc uk " },
            };
            var task = PostService.AddAsync(post);
            task.Wait();
            Assert.IsTrue(PostService.Result.Succeeded, PostService.Result.Errors.FirstOrDefault());
        }


        [TestMethod]
        public void Find()
        {
            var task = PostService.FindByIdAsync(Guid.Parse("33e63916-b4ef-43c4-b9f8-908ca90eecd1"));
            task.Wait();
            Assert.IsNotNull(task.Result, PostService.Result.Errors.FirstOrDefault());
            Assert.IsTrue(task.Result.Tags.Any(), "No Tags.");
            Debug.WriteLine(task.Result.Tags.Select(x => x.Name).Aggregate((i, s) => i + ", " + s));
        }


    }
}
