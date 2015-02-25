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
    public class CommentServiceTest
    {
        private ICommentService _commentService;
        public ICommentService CommentService
        {
            get { return _commentService ?? (_commentService = ServiceIOC.Get<ICommentService>()); }
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
            //var comment = new CommentAddModel
            //{
            //    Body = "مطلبی روز گذشته کار شد در مورد امنیت سیستم عاملها که نویسنده زومیت بخش اتفاقاً مهمی رو ترجمه نکرده (امیدوارم قصد قبلی و ترولینگ در کار نباشه) با توجه به تحقیر بعضی کاربران نسبت به کاربران OSx و iOS و همچنین لینوکس مناسبتره زومیت در یک مطلب روشنگری کنه",
            //    Email = "behnam@behi.com",
            //    Name = "بهنام",
            //    IsConfirmed = true,
            //    PostId = Guid.Parse("20b7d55e-4851-4873-92da-377f1b8f1d43"),
            //};
            var comment = new CommentAddModel
            {
                Body = "موافقم.",
                Email = "behnam@behi.com",
                Name = "سلام",
                IsConfirmed = true,
                PostId = Guid.Parse("20b7d55e-4851-4873-92da-377f1b8f1d43"),
                ParentId = Guid.Parse("260272d6-ba80-4fc9-a77a-a4b12d8d61bf"),
            };
            var task = CommentService.AddAsync(comment);
            task.Wait();
            Assert.IsTrue(CommentService.Result.Succeeded, CommentService.Result.Errors.FirstOrDefault());
        }


    }
}
