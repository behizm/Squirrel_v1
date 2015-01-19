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
            //var task = UserService.CreateAsync("behi8303", "behi8303@yahoo.com", "123456");
            var task = UserService.CreateAsync("admin", "admin@admin.com", "123456");
            task.Wait();
            Assert.IsTrue(UserService.Result.Succeeded, UserService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void UpdateUser()
        {
            var task1 = UserService.FindByUsernameAsync("behi8303");
            task1.Wait();
            Assert.IsNotNull(task1.Result, UserService.Result.Errors.FirstOrDefault());

            var task = UserService.UpdateAsync(task1.Result.Id, task1.Result.Username, "behi8303@yahoo.com");
            task.Wait();
            Assert.IsTrue(UserService.Result.Succeeded, UserService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void ChangePassword()
        {
            var task = UserService.ChangePasswordAsync("behi8303", "1234567", "123456");
            task.Wait();
            Assert.IsTrue(UserService.Result.Succeeded, UserService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Login()
        {
            var task = UserService.LoginAsync("behi8303", string.Empty, "123456");
            task.Wait();
            Assert.IsTrue(task.Result, UserService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void ChangeAdmin()
        {
            var task = UserService.ChangeAdminAsync(Guid.Parse("a4ee56af-6d44-4ca2-a049-94406e9c6a84"), true);
            task.Wait();
            Assert.IsTrue(UserService.Result.Succeeded, UserService.Result.Errors.FirstOrDefault());
        }


    }
}
