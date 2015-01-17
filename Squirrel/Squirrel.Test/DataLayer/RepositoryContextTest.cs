using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Data;
using Squirrel.Domain.Enititis;

namespace Squirrel.Test.DataLayer
{
    [TestClass]
    public class RepositoryContextTest
    {
        private IRepositoryContext _repositoryContext;
        public IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory",
            //    Path.Combine(context.TestDeploymentDir, string.Empty));
            var stringList = AppDomain.CurrentDomain.BaseDirectory.Split('\\').ToList();
            stringList[stringList.Count - 3] = "Squirrel.Web";
            stringList[stringList.Count - 2] = "App_Data";
            stringList.RemoveAt(stringList.Count - 1);
            var path = stringList.Aggregate((i, j) => i + "\\" + j);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        [TestMethod]
        public void AddData()
        {
            var user = new User
            {
                AccessFailed = 0,
                Email = "behi8303@yahoo.com",
                PasswordHash = "121212",
                Username = "behi8303",
            };
            var task = RepositoryContext.CreateAsync(user);
            task.Wait();
            Assert.IsTrue(RepositoryContext.OperationResult.Succeeded, RepositoryContext.OperationResult.Errors.FirstOrDefault());
        }

        [TestMethod]
        public void UpdateData()
        {
            var guid = Guid.Parse("0956AFED-2A60-4B50-9E50-C2FA49A306DC");
            var task1 = RepositoryContext.RetrieveAsync<User>(x => x.Id == guid);
            task1.Wait();
            Assert.IsTrue(RepositoryContext.OperationResult.Succeeded, "1: " + RepositoryContext.OperationResult.Errors.FirstOrDefault());
            var user = task1.Result;
            user.Username = "behi8304";
            var task2 = RepositoryContext.UpdateAsync(user);
            task2.Wait();
            Assert.IsTrue(RepositoryContext.OperationResult.Succeeded, "2: " + RepositoryContext.OperationResult.Errors.FirstOrDefault());
        }


    }
}
