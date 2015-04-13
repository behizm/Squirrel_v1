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
    public class TagServiceTest
    {
        private ITagService _tagService;
        public ITagService TagService
        {
            get { return _tagService ?? (_tagService = ServiceIOC.Get<ITagService>()); }
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
        public void Weight()
        {
            var weightTask = TagService.TagsWithWeightAsync(
                new OrderingModel<TagWeightModel, int>
                {
                    IsAscending = false,
                    OrderByKeySelectorFunc = x => x.Weight,
                    Skip = 0,
                    Take = 10,
                });
            weightTask.Wait();
            var weight = weightTask.Result;
            Assert.IsTrue(TagService.Result.Succeeded, TagService.Result.Errors.FirstOrDefault());
            
        }

    }
}
