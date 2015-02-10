using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Web.Controllers;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class PostsController : BaseController
    {
        public async Task<ActionResult> Add(Guid id)
        {
            var topic = await TopicService.FindByIdAsync(id);
            if (topic == null)
            {
                ViewBag.ErrorMessage = "عنوان مطلب مشخص نیست.";
                return PartialView("_Message");
            }

            ViewBag.TopicTitle = topic.Title;
            ViewBag.TopicId = topic.Id;
            return View();
        }
    }
}