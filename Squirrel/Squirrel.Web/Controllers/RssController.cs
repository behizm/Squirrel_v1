using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ExtensionMethods;
using Squirrel.Utility.Helpers;
using Squirrel.Utility.Rss;
using Squirrel.Web.Models;

namespace Squirrel.Web.Controllers
{
    public class RssController : BaseController
    {
        public ActionResult Index()
        {
            return MakeRss(CachedAppData.LastPublishedTopics.Items.Skip(0).Take(10), string.Empty);
        }

        public FileResult File()
        {
            return MakeRssFile(CachedAppData.LastPublishedTopics.Items.Skip(0).Take(10), string.Empty);
        }

        public async Task<ActionResult> Category(string id)
        {
            if (id.IsNothing())
            {
                return null;
            }

            var items = await CategoryService.PublishedTopicsAsync(id, true, 0, 10);
            if (items == null || items.List == null)
            {
                return null;
            }

            return MakeRss(items.List, string.Format("گروه {0}", id));
        }

        public async Task<ActionResult> Author(string id)
        {
            Guid userId;
            var guidResult = Guid.TryParse(id, out userId);
            if (!guidResult)
            {
                return null;
            }

            var user = await UserService.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var items = await UserService.PublishedTopicsAsync(user.Id, 0, 10);
            if (items == null || items.List == null)
            {
                return null;
            }

            return MakeRss(items.List, user.ShowName());
        }



        private SyndicationFeed GetFeed(IEnumerable<Topic> topics, string feedType)
        {
            var feedItems =
                topics.Select(x =>
                    new SyndicationItem(
                        x.Title,
                        x.Abstarct(),
                        new Uri(Url.Action("Index", "Issue", new { id = x.IssueId }, Request.Url.Scheme)),
                        x.IssueId,
                        x.PublishDate ?? x.CreateDate))
                    .ToList();

            return new SyndicationFeed(
                feedType.IsNotNothing() ? string.Format("سایت سنجاب، {0}", feedType) : "سایت سنجاب",
                "از آخرین مطالب سایت باخبر شوید.",
                new Uri(Url.Action("Index", "Home", null, Request.Url.Scheme)),
                "",
                DateTime.Now,
                feedItems);
        }

        private ActionResult MakeRss(IEnumerable<Topic> topics, string feedType)
        {
            var feed = GetFeed(topics, feedType);
            return new RssActionResult(feed);
        }

        private FileResult MakeRssFile(IEnumerable<Topic> topics, string feedType)
        {
            var feed = GetFeed(topics, feedType);
            return new RssFileResult(feed);
        }

    }
}