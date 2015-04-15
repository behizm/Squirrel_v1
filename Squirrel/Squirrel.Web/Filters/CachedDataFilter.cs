using System.Web.Mvc;
using Squirrel.Web.Models;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace Squirrel.Web.Filters
{
    public class UpdateCachedDataFilter : ActionFilterAttribute
    {
        public UpdateCachedDataFilter()
            : this(UpdateCachedDataType.OnActionExecuted)
        {
        }

        public UpdateCachedDataFilter(UpdateCachedDataType updateType)
        {
            UpdateType = updateType;
        }

        public UpdateCachedDataType UpdateType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (UpdateType == UpdateCachedDataType.OnActionExecuting)
            {
                (new CachedAppDataMethods()).SyncAll();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (UpdateType == UpdateCachedDataType.OnActionExecuted)
            {
                (new CachedAppDataMethods()).SyncAll();
            }
        }
    }

    public enum UpdateCachedDataType
    {
        OnActionExecuting,
        OnActionExecuted
    }
}