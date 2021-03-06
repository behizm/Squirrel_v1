﻿using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Squirrel.Web.Models;

namespace Squirrel.Web
{
    public class MvcApplication : HttpApplication
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += ApplicationAuthenticateRequest;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            Context.User = Thread.CurrentPrincipal = new SqPrincipal(User.Identity.Name);
        }

        private void ApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            Context.User = Thread.CurrentPrincipal = new SqPrincipal(User.Identity.Name);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // ---
            (new CachedAppDataMethods()).SyncAll();
        }
    }
}
