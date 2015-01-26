using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Squirrel.Web.Models
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new ISqPrincipal User
        {
            //get { return (ISqPrincipal)base.User; }
            get { return new SqPrincipal(base.User.Identity.Name); }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new ISqPrincipal User
        {
            //get { return (ISqPrincipal)base.User; }
            get { return new SqPrincipal(base.User.Identity.Name); }
        }
    }
}