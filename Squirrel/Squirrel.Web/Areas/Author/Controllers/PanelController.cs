using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Web.Controllers;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class PanelController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var commentsStatsTask = CommentService.Statistics(new CommentStatisticsModel { AuthorId = User.Identity.UserId });
            var topicCountTask = TopicService.CountAsync(new TopicSearchModel { Username = User.Identity.Name });
            var fileCountTask = FileService.CountAsync(new FileSearchModel { Username = User.Identity.Name });
            var lastTopicsTask = TopicService2.SearchAsync(new TopicSearchModel { Username = User.Identity.Name },
                new OrderingModel<Topic, DateTime>
                {
                    IsAscending = false,
                    OrderByKeySelector = x => x.CreateDate,
                    Skip = 0,
                    Take = 3,
                });
            var lastCommentsTask = CommentService2.SearchAsync(new CommentSearchModel { AuthorId = User.Identity.UserId },
                new OrderingModel<Comment, DateTime>
                {
                    IsAscending = false,
                    OrderByKeySelector = x => x.CreateDate,
                    Skip = 0,
                    Take = 3,
                });

            var commentsStats = await commentsStatsTask;
            if (commentsStats == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = CommentService.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            var topicCount = await topicCountTask;
            if (topicCount == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            var fileCount = await fileCountTask;
            if (fileCount == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = FileService.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            var lastTopics = await lastTopicsTask;
            if (lastTopics == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = TopicService2.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            var lastComments = await lastCommentsTask;
            if (lastComments == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = CommentService2.Result.Errors.FirstOrDefault(),
                };
                return View("HandledError");
            }

            ViewBag.CommentsCount = commentsStats;
            ViewBag.TopicsCount = topicCount;
            ViewBag.FilesCount = fileCount;
            ViewBag.LastTopics = lastTopics;
            ViewBag.LastComments = lastComments;
            return View();
        }
    }
}