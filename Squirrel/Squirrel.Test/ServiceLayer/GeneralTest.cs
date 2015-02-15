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
    public class GeneralTest
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

        private ICategoryService _categoryService;
        public ICategoryService CategoryService
        {
            get { return _categoryService ?? (_categoryService = ServiceIOC.Get<ICategoryService>()); }
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
        public void Initializing()
        {
            // Add User
            var usertask = UserService.CreateAsync("behi8303", "behi8303@yahoo.com", "123456");
            usertask.Wait();
            Assert.IsTrue(UserService.Result.Succeeded, UserService.Result.Errors.FirstOrDefault());

            var user = UserService.FindByUsernameAsync("behi8303").Result;
            Assert.IsNotNull(user, UserService.Result.Errors.FirstOrDefault());

            // Add Category
            var cattask = CategoryService.AddAsync("Game", "", "");
            cattask.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());

            var cat = CategoryService.FindByNameAsync("Game").Result;
            Assert.IsNotNull(cat, CategoryService.Result.Errors.FirstOrDefault());

            // Add Topic
            var topic = new TopicAddModel
            {
                CategoryId = cat.Id,
                PostsOrdering = PostsOrdering.Newer,
                Title = "Game Topic 1",
                Username = "behi8303"
            };
            var topictask = TopicService.AddAsync(topic);
            topictask.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());

            var topics =
                TopicService.SearchAsync(new TopicSearchModel { Title = "Game Topic 1" },
                    new OrderingModel<Topic> { OrderByKeySelector = x => x.Title })
                    .Result;
            Assert.IsNotNull(topics, TopicService.Result.Errors.FirstOrDefault());
            Assert.IsTrue(topics.Any(), "No Topic.");

            // Add Post
            var post = new PostAddModel
            {
                Body = "This is test of post content for game Topic.",
                TopicId = topics.First().Id,
                Username = user.Username,
                Tags = new List<string> { "game", "shooter", "fps" },
            };
            var task = PostService.AddAsync(post);
            task.Wait();
            Assert.IsTrue(PostService.Result.Succeeded, PostService.Result.Errors.FirstOrDefault());
        }


    }
}
