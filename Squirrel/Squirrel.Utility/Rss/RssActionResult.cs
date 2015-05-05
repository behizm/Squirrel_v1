using System;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Squirrel.Utility.Rss
{
    public class RssActionResult : ActionResult
    {
        private readonly SyndicationFeed _feed;

        public RssActionResult(SyndicationFeed feed)
        {
            _feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";
            var rssFormatter = new Rss20FeedFormatter(_feed);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }


    public class RssFileResult : FileResult
    {
        private readonly SyndicationFeed _feed;

        public RssFileResult(SyndicationFeed feed)
            : base("application/rss+xml")
        {
            _feed = feed;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            using (var writer = XmlWriter.Create(response.OutputStream))
            {
                _feed.GetRss20Formatter().WriteTo(writer);
            }
        }
    }
}
