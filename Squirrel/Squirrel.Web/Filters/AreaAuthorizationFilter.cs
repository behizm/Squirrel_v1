using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Squirrel.Web.Models;

namespace Squirrel.Web.Filters
{
    public class AreaAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!filterContext.RouteData.DataTokens.ContainsKey("area"))
                return;

            var area = filterContext.RouteData.DataTokens["area"].ToString();

            if (string.IsNullOrEmpty(area))
                return;

            if (area.ToLower() != "admin" && area.ToLower() != "author")
                return;

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.Url == null)
                {
                    RedirectRoute(filterContext, null, "account", "login");
                    return;
                }

                RedirectRoute(filterContext, null, "account", "login", "returnUrl", filterContext.HttpContext.Request.Url.LocalPath);
                return;
            }

            if (area.ToLower() != "admin")
                return;

            var isAdmin = ((ISqPrincipal)filterContext.HttpContext.User).Identity.IsAdmin;
            if (!isAdmin.HasValue || !isAdmin.Value)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    RedirectRoute(filterContext, null, "Error", "AccessDeniedPartial");
                }
                RedirectRoute(filterContext, null, "Error", "AccessDenied");
            }


        }

        private static void RedirectRoute(ActionExecutingContext filterContext, string area, string controller, string action)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Area", area},
                        {"Controller", controller},
                        {"Action", action}
                    });
        }

        private static void RedirectRoute(ActionExecutingContext filterContext, string area, string controller, string action, string argName, string argValue)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Area", area},
                        {"Controller", controller},
                        {"Action", action},
                        {argName, argValue}
                    });
        }
    }
}