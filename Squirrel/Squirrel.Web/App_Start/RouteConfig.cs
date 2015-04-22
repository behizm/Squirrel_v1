﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Squirrel.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new {botdetect = @"(.*)BotDetectCaptcha\.ashx"});

            routes.MapRoute(
                name: "I&Id",
                url: "I/{id}",
                defaults: new { controller = "Issue", action = "Index" },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Controller&Id",
                url: "Issue/{id}",
                defaults: new { controller = "Issue", action = "Item" },
                constraints: new { id = @"[a-zA-Z0-9]{8}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{12}" }
            );

            routes.MapRoute(
                name: "Category",
                url: "Category/{id}",
                defaults: new { controller = "Issues", action = "Category" }
            );

            routes.MapRoute(
                name: "User",
                url: "User/{id}",
                defaults: new { controller = "Issues", action = "User" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
