using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Squirrel.Web.Startup))]
namespace Squirrel.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
