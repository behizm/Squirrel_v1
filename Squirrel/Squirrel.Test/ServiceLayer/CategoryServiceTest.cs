using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Domain.Enititis;
using Squirrel.Service;

namespace Squirrel.Test.ServiceLayer
{
    [TestClass]
    public class CategoryServiceTest
    {
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
        public void Create()
        {
            var task = CategoryService.AddAsync("movie", "", "");
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void ChangeName()
        {
            var task = CategoryService.ChangeNameAsync("auDio", "Movie");
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void ChangeParentName()
        {
            var task = CategoryService.ChangeParentAsync("movie", "");
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Delete()
        {
            var task = CategoryService.DeleteAsync(Guid.Parse("7820b8fa-666e-4e5c-af3e-eefe82954a58"));
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Replace()
        {
            var task = CategoryService.ReplaceAsync("movie", "game");
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Childs()
        {
            var task = CategoryService.ChildsNameAsync("game");
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
            Assert.Fail(task.Result.Aggregate((i, x) => i + ", " + x));
        }

        [TestMethod]
        public void Topics()
        {
            var task = CategoryService.TopicsAsync("game", false);
            task.Wait();
            Assert.IsTrue(CategoryService.Result.Succeeded, CategoryService.Result.Errors.FirstOrDefault());
            Assert.Fail(task.Result.Select(x => x.Title).Aggregate((i, x) => i + ", " + x));
        }


    }
}
