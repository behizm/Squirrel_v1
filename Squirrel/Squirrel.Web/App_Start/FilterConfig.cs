using System.Web.Mvc;
using Squirrel.Web.Filters;

namespace Squirrel.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.AreaAuthorizationFilter());

            filters.Add(new LogFilter());
            filters.Add(new ExceptionFilter());
        }
    }
}
