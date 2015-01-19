using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Domain.Enititis;
using Squirrel.Service;

namespace Squirrel.Test.ServiceLayer
{
    [TestClass]
    public class TopicServiceTest
    {
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
            var topic = new Topic
            {
                CategoryId = Guid.Parse("88e71d5f-ce0d-4e9e-ae1b-fc0a23e6ae63"),
                FirstPost = FirstPostType.Newer,
                Title = "Game Topic 4",
                UserId = Guid.Parse("0df93e6b-9881-4220-8e05-d67960b4cedf"),
            };
            var task = TopicService.AddAsync(topic);
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
        }


    }
}
