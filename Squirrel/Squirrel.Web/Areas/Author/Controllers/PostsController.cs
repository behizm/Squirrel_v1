using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.FarsiTools;
using Squirrel.Utility.Helpers;
using Squirrel.Web.Controllers;
using Squirrel.Web.Filters;
using WebGrease.Css.Extensions;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class PostsController : BaseController
    {
        public async Task<ActionResult> Item(Guid id)
        {
            var post = await PostService.FindByIdAsync(id);
            if (post == null)
            {
                ViewBag.ErrorMessage = "این مطلب وجود ندارد.";
                return PartialView("_Message");
            }

            ViewBag.CurrentPost = post;
            ViewData.Model = new PostEditModel
            {
                Abstract = post.Abstract,
                Body = post.Body,
                Id = post.Id,
                Attachments = post.Attachments.Select(x => x.Id).ToList(),
                HeaderImageId = post.HeaderImageId,
                Tags = post.Tags.Select(x => x.Name).OrderBy(x => x).ToList(),
                TopicId = post.TopicId,
                IsPublic = post.IsPublic,
                FlatedTags =
                    post.Tags.Any()
                        ? "#" + post.Tags.Select(x => x.Name).Aggregate((i, s) => i + "#" + s) + "#"
                        : "#",
                FlatedAttachments =
                    post.Attachments.Any()
                        ? "#" + post.Attachments.Select(x => x.Id.ToString()).Aggregate((i, s) => i + "#" + s) + "#"
                        : "#",
                PublishPersianDate =
                    post.PublishDate.HasValue
                        ? ((PersianDate)post.PublishDate.Value).ToStringDateTime(timeFormat: PersianTimeFormat.HH_MM_SS)
                        : string.Empty,
            };
            return View();
        }

        [UpdateCachedDataFilter]
        public async Task<JsonResult> Edit(PostEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                JsonRequestBehavior.AllowGet);
            }

            model.Tags = new List<string>();
            if (!string.IsNullOrEmpty(model.FlatedTags) && !string.IsNullOrWhiteSpace(model.FlatedTags))
            {
                var tags = model.FlatedTags.Split('#');
                if (tags.Any())
                {
                    tags.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.Trim()))
                        {
                            model.Tags.Add(x.Trim());
                        }
                    });
                }
            }

            model.Attachments = new List<Guid>();
            if (!string.IsNullOrEmpty(model.FlatedAttachments) && !string.IsNullOrWhiteSpace(model.FlatedAttachments))
            {
                var attachs = model.FlatedAttachments.Split('#');
                if (attachs.Any())
                {
                    attachs.ForEach(x =>
                    {
                        if (string.IsNullOrEmpty(x.Trim()))
                            return;

                        Guid attach;
                        if (Guid.TryParse(x.Trim(), out attach))
                        {
                            model.Attachments.Add(attach);
                        }
                    });
                }
            }

            if (model.PublishPersianDate.IsNotEmpty())
            {
                var date = model.PublishPersianDate.Split(' ')[0];
                var time = model.PublishPersianDate.Split(' ')[1];
                model.PublishDateTime = new PersianDate(date, time);
            }

            model.Username = User.Identity.Name;
            await PostService.EditAsync(model);
            if (!PostService.Result.Succeeded)
                return Json(new {result = false, message = PostService.Result.Errors.FirstOrDefault()},
                    JsonRequestBehavior.AllowGet);

            var publishDate = "";
            if (model.IsPublic)
            {
                if (model.PublishPersianDate.IsNotEmpty())
                {
                    publishDate = model.PublishPersianDate.FaDigit();
                }
                else
                {
                    publishDate =
                        ((PersianDate) DateTime.Now).ToStringDateTime(timeFormat: PersianTimeFormat.HH_MM_SS)
                            .FaDigit();
                }
            }
            return
                Json(
                    new
                    {
                        result = true,
                        message = "مطلب با موفقیت ویرایش شد.",
                        date = ((PersianDate)DateTime.Now).ToStringDateTime(), 
                        publishDate,
                    }, JsonRequestBehavior.AllowGet);
        }

        [UpdateCachedDataFilter]
        public async Task<JsonResult> Remove(Guid id)
        {
            var model = new PostRemoveModel
            {
                Id = id,
                Username = User.Identity.Name,
            };
            await PostService.DeleteAsync(model);
            if (PostService.Result.Succeeded)
            {
                return Json(new { result = true, message = "مطلب با موفقیت حذف شد." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = PostService.Result.Errors.FirstOrDefault() },
                JsonRequestBehavior.AllowGet);
        }
    }
}