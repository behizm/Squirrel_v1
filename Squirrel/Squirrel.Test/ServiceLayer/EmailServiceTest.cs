using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Domain.ViewModels;
using Squirrel.Service;
using Squirrel.Service.Interfaces;

namespace Squirrel.Test.ServiceLayer
{
    [TestClass]
    public class EmailServiceTest
    {
        private IEmailService _emailService;
        public IEmailService EmailService
        {
            get { return _emailService ?? (_emailService = ServiceIOC.Get<IEmailService>()); }
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
        public void Send()
        {
            var dateNow = DateTime.Now;
            var emailModel = new EmailSendModel
            {
                Body = "for testing mail service",
                Subject = "squirrel test : " + dateNow.Day + dateNow.Hour + dateNow.Minute + dateNow.Second,
                ToAddress = "behnam.zeighami@gmail.com",
            };
            var task = EmailService.SendAsync(emailModel);
            task.Wait();
            Assert.IsTrue(EmailService.Result.Succeeded, EmailService.Result.Errors.FirstOrDefault());
        }

    }
}
