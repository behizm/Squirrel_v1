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
    public class ConfigServiceTest
    {
        private IConfigService _configService;
        public IConfigService ConfigService
        {
            get { return _configService ?? (_configService = ServiceIOC.Get<IConfigService>()); }
        }

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
        public void Add()
        {
            var user = UserService.FindByUsernameAsync("behi8303").Result;
            Assert.IsNotNull(user, UserService.Result.Errors.FirstOrDefault());

            var task = ConfigService.AddAsync("Key 1 ", " Val 1", user.Id);
            task.Wait();
            Assert.IsTrue(ConfigService.Result.Succeeded, ConfigService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Edit()
        {
            var user = UserService.FindByUsernameAsync("behi8303").Result;
            Assert.IsNotNull(user, UserService.Result.Errors.FirstOrDefault());

            var task = ConfigService.EditAsync("Key 1 ", " Val 2", user.Id);
            task.Wait();
            Assert.IsTrue(ConfigService.Result.Succeeded, ConfigService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Delete()
        {
            var user = UserService.FindByUsernameAsync("behi8303").Result;
            Assert.IsNotNull(user, UserService.Result.Errors.FirstOrDefault());

            var task = ConfigService.DeleteAsync("Key 1 ", user.Id);
            task.Wait();
            Assert.IsTrue(ConfigService.Result.Succeeded, ConfigService.Result.Errors.FirstOrDefault());
        }

    }
}
