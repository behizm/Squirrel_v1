﻿using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using BotDetect.Web.UI.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ExtensionMethods;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class IssueController : BaseController
    {

        public async Task<ActionResult> Index(string id)
        {
            var topic = await TopicService.FindByIssueIdAsync(id);
            if (topic == null || !topic.IsPublished)
            {
                return View("NotFound");
            }

            const int count = 3;
            var ralatedListModelTask = CategoryService.PublishedTopicsAsync(topic.Category.Name, true, 0, count + 1);
            var prevTopicTask =
                TopicService.SearchAsync(
                    new TopicSearchModel
                    {
                        PublishDateTo = topic.PublishDate,
                        IsPublished = true
                    },
                    new OrderingModel<Topic, DateTime?>
                    {
                        IsAscending = false,
                        OrderByKeySelector = x => x.PublishDate,
                        Skip = 1,
                        Take = 1,
                    });
            var nextTopicTask =
                TopicService2.SearchAsync(
                    new TopicSearchModel
                    {
                        PublishDateFrom = topic.PublishDate,
                        IsPublished = true
                    },
                    new OrderingModel<Topic, DateTime?>
                    {
                        IsAscending = true,
                        OrderByKeySelector = x => x.PublishDate,
                        Skip = 1,
                        Take = 1,
                    });

            topic.Posts = topic.Posts.PublicOrderedPosts(topic.PostsOrdering);

            var relatedListModel = await ralatedListModelTask;
            var related = relatedListModel.List;
            related.Remove(related.Find(x => x.Id == topic.Id));
            if (related.Count < count)
            {
                var lack = CachedAppData.LastPublishedTopics.Items.Where(x => x.Id != topic.Id && related.All(r => r.Id != x.Id)).Take(count - related.Count);
                related.AddRange(lack);
            }
            else if (related.Count > count)
            {
                var more = related.Skip(count).Take(related.Count - count).ToList();
                more.ForEach(x => related.Remove(x));
            }
            ViewBag.RelatedTopics = related;

            var prevTopic = await prevTopicTask;
            if (prevTopic != null && prevTopic.Any())
            {
                ViewBag.PreviousTopic = prevTopic.First();
            }

            var nextTopic = await nextTopicTask;
            if (nextTopic != null && nextTopic.Any())
            {
                ViewBag.NextTopic = nextTopic.First();
            }


            await TopicService.PlusView(topic.Id);
            if (CachedAppData.PopularPublishedTopics.LastUpdate < DateTime.Now.AddMinutes(-30))
            {
                await (new CachedAppDataMethods()).SyncPopularPublishedTopicsAsync();
            }
            return View("Item", topic);
        }

        [Authorize]
        public async Task<ActionResult> Preview(Guid id)
        {
            var topic = await TopicService.FindByIdAsync(id);
            if (topic == null)
            {
                return View("NotFound");
            }

            if (!User.Identity.IsAdmin && topic.Owner.Username != User.Identity.Name.ToLower())
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطای دسترسی",
                    Message = "شما دسترسی لازم برای مشاهده این صفحه را ندارید.",
                };
                return View("HandledError");
            }

            if (topic.Posts == null || !topic.Posts.Any())
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطای محتوا",
                    Message = "این عنوان محتوای کافی برای نمایش را ندارد.",
                };
                return View("HandledError");
            }

            topic.Posts = topic.Posts.OrderByPostOrdering(topic.PostsOrdering);
            ViewData.Model = topic;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaText", "CaptchaId", "عبارت امنیتی را اشتباه وارد کردید.")]
        public async Task<JsonResult> AddComment(CommentAddPublicModel model)
        {
            Thread.Sleep(5000);

            MvcCaptcha.ResetCaptcha("CaptchaId");
            if (!ModelState.IsValid)
            {
                var captchaError = "";
                if (ModelState["CaptchaText"].Errors.Any())
                {
                    captchaError = "عبارت امنیتی را اشتباه وارد کردید.";
                }

                return Json(new { result = false, captchaError, message = ServiceMessages.General_LackOfInputData },
                    JsonRequestBehavior.AllowGet);
            }

            var item = new CommentAddModel
            {
                Body = model.Body,
                Email = model.Email,
                IsConfirmed = false,
                IsRead = false,
                Name = model.Name,
                PostId = model.PostId,
                Username = User.Identity.IsAuthenticated ? User.Identity.Name : null,
            };

            await CommentService.AddAsync(item);
            if (CommentService.Result.Succeeded)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CommentService.Result.Errors.FirstOrDefault() },
                    JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Attachment(string token)
        {
            var errorModel = new ErrorViewModel
            {
                Topic = "دانلود فایل امکان پذیر نیست!",
                Message = "به صفحه مطلب مراجعه کرده و فایل را دوباره دانلود کنید.",
            };

            var code = RsaClass.Decrypt(token);
            if (code.IsNothing())
            {
                return View("HandledError", errorModel);
            }

            var codeArray = code.Split(':');
            if (codeArray.Length != 2)
            {
                return View("HandledError", errorModel);
            }

            Guid fileId;
            var parseResult = Guid.TryParse(codeArray[0], out fileId);
            if (!parseResult)
            {
                return View("HandledError", errorModel);
            }

            DateTime createTime;
            try
            {
                var ticks = Convert.ToInt64(codeArray[1]);
                createTime = new DateTime(ticks);
            }
            catch (Exception)
            {
                return View("HandledError", errorModel);
            }

            if ((DateTime.Now - createTime).TotalMinutes > 45)
            {
                errorModel.Topic = "اعتبار این لینک به اتمام رسیده است!";
                return View("HandledError", errorModel);
            }

            var file = await FileService.FindByIdAsync(fileId);
            if (file == null)
            {
                errorModel.Topic = "هیچ فایلی یافت نشد!";
                errorModel.Message = "فایل مورد نظر یا پاک شده یا منتقل گردیده است.";
                return View("HandledError", errorModel);
            }

            var filePath = System.Web.HttpContext.Current.Server.MapPath(file.Address);
            var localFile = new FileInfo(filePath);
            if (!localFile.Exists)
            {
                errorModel.Topic = "هیچ فایلی یافت نشد!";
                errorModel.Message = "فایل مورد نظر یا پاک شده یا منتقل گردیده است.";
                return View("HandledError", errorModel);
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, MediaTypeNames.Application.Octet, file.Filename);
        }
    }
}