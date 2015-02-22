using System.Web.Mvc;
using Squirrel.Domain.ViewModels;
using Squirrel.Service;

namespace Squirrel.Web.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string area = null;
            if (filterContext.RouteData.DataTokens.ContainsKey("area"))
            {
                area = filterContext.RouteData.DataTokens["area"].ToString();
            }

            var log = new LogAddModel
            {
                Area = area,
                Action = filterContext.RequestContext.RouteData.Values["action"].ToString(),
                Controller = filterContext.RequestContext.RouteData.Values["controller"].ToString(),
                FullUrl = filterContext.HttpContext.Request.Url != null ? filterContext.HttpContext.Request.Url.AbsoluteUri : null,
                IsAjax = filterContext.HttpContext.Request.IsAjaxRequest(),
                Ip = filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"],
                Port = filterContext.HttpContext.Request.ServerVariables["REMOTE_PORT"],
                IsPostMethod = filterContext.HttpContext.Request.HttpMethod.ToLower() == "post",
                ReferredHost = filterContext.HttpContext.Request.UrlReferrer != null ? filterContext.HttpContext.Request.UrlReferrer.Host : null,
                ReferredUrl = filterContext.HttpContext.Request.UrlReferrer != null ? filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri : null,
                UserAgent = filterContext.HttpContext.Request.UserAgent,
                Username = filterContext.HttpContext.User.Identity.Name,
            };
            var logService = ServiceIOC.Get<ILogService>();
            logService.AddAsync(log);
        }
    }
}