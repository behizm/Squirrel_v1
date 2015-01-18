using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Domain.Enititis;
using Squirrel.Service;

namespace Squirrel.Test.ServiceLayer
{
    [TestClass]
    public class ProfileServiceTest
    {
        private IProfileService _profileService;
        public IProfileService ProfileService
        {
            get { return _profileService ?? (_profileService = ServiceIOC.Get<IProfileService>()); }
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
            var profile = new Profile
            {
                Firstname = "Behnam",
                Lastname = "Zeighami",
            };
            var task = ProfileService.CreateAsync(Guid.Parse("0bae394c-6384-4918-b877-8bd5b0e01adb"), profile);
            task.Wait();
            Assert.IsTrue(ProfileService.Result.Succeeded, ProfileService.Result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Update()
        {
            var profile = new Profile
            {
                Firstname = "Behnam",
                Lastname = "Zeighamy",
            };
            var task = ProfileService.UpdateAsync(Guid.Parse("0bae394c-6384-4918-b877-8bd5b0e01adb"), profile);
            task.Wait();
            Assert.IsTrue(ProfileService.Result.Succeeded, ProfileService.Result.Errors.FirstOrDefault());
        }


    }
}
