using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Service;

namespace Squirrel.Test.ServiceLayer
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService _userService;
        public IUserService UserService
        {
            get { return _userService ?? (_userService = ServiceIOC.Get<IUserService>()); }
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
        public void CreateUser()
        {
            var task = UserService.CreateAsync("behi8303", "behi8303@yahoo.com", "123456");
            task.Wait();
            Assert.IsTrue(UserService.Result.Succeeded, UserService.Result.Errors.FirstOrDefault());
        }
    }
}
