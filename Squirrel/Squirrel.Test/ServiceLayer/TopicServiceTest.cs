using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
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
            var topic = new TopicAddModel
            {
                CategoryId = Guid.Parse("4026c6bc-4883-4325-8064-b26bbd977f16"),
                PostsOrdering = PostsOrdering.Newer,
                Title = "Game Topic 1",
                UserId = Guid.Parse("03983bdf-5143-41a1-b4b4-9f5174c81d3e"),
            };
            var task = TopicService.AddAsync(topic);
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Change()
        {
            var topic = new TopicEditModel
            {
                CategoryId = Guid.Parse("95d29716-a0e3-4a3b-9376-03cb04731b04"),
                PostsOrdering = PostsOrdering.Newer,
                Title = "Movie Topic 2",
                Id = Guid.Parse("b56b696b-d21f-4a15-960b-791fdd92b2f3"),
            };
            var task = TopicService.EditAsync(topic, Guid.Parse("a4ee56af-6d44-4ca2-a049-94406e9c6a84"));
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Delete()
        {
            var model = new TopicDeleteModel
            {
                Id = Guid.Parse("b56b696b-d21f-4a15-960b-791fdd92b2f3"),
            };
            var task = TopicService.DeleteAsync(model, Guid.Parse("935bf015-5bf7-4219-bb4b-c0a8a82b9517"));
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Search()
        {
            var model = new TopicSearchModel
            {
                Title = "game",
            };
            var ordering = new OrderingModel<Topic>
            {
                KeySelector = t => t.Title,
                IsAscending = false,
            };
            var task = TopicService.SearchAsync(model, ordering);
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
            Debug.WriteLine(task.Result.Select(x => x.Title).Aggregate((i, s) => i + "\n" + s));
        }

        [TestMethod]
        public void Count()
        {
            var model = new TopicSearchModel
            {
                Title = "game",
            };
            var task = TopicService.CountAsync(model);
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
            Debug.WriteLine(task.Result.ToString());
        }

        [TestMethod]
        public void Publish()
        {
            var task = 
                TopicService.PublishAsync(
                Guid.Parse("d659b09f-d24e-47ad-8a41-bff66e62d7f6"), 
                Guid.Parse("935bf015-5bf7-4219-bb4b-c0a8a82b9517"));
            //var task = TopicService.PublishAsync(Guid.Parse("d659b09f-d24e-47ad-8a41-bff66e62d7f6"), Guid.Parse("a4ee56af-6d44-4ca2-a049-94406e9c6a84"));
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void UnPublish()
        {
            var task = 
                TopicService.UnPublishAsync(
                Guid.Parse("d659b09f-d24e-47ad-8a41-bff66e62d7f6"), 
                Guid.Parse("935bf015-5bf7-4219-bb4b-c0a8a82b9517"));
            task.Wait();
            Assert.IsTrue(TopicService.Result.Succeeded, TopicService.Result.Errors.FirstOrDefault());
        }


    }
}
