using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Squirrel.Web.Areas.Author
{
    public class AuthorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Author";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Author_default",
                "Author/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}